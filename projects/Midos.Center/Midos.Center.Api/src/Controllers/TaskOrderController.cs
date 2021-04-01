using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Midos.Center.Entities;
using Midos.Center.Events;
using Midos.Domain;
using System;
using System.Linq;
using Midos.Center.Utils;

namespace Midos.Center.Controllers
{
  public class TaskOrderInternalController: BaseController
  {
    private const string Group = "midos.tas.tasks";

    private ICapPublisher _cap;

    private DomainContext _domain;

    private TaskService _tasks;

    public TaskOrderInternalController(
      DomainContext domain,
      ICapPublisher cap,
      TaskService tasks
    ) {
      _cap = cap;
      _domain = domain;
      _tasks = tasks;
    }

    [HttpPost("/midos/tasks/create")]
    public INotifyResult<IMessageObject> CreateTaskOrder([FromBody] TaskOrderCreate param)
    {
      var result = NotifyResult.FromVoid();

      HandleTaskOrderCreate(param);

      return result.Success("任务已创建");
    }

    [HttpPost("/midos/subtasks/create")]
    public INotifyResult<IMessageObject> CreateSubtaskOrder([FromBody] SubtaskOrderCreate param)
    {
      var result = NotifyResult.FromVoid();

      HandleSubtaskOrderCreate(param);

      return result.Success("子任务已创建");
    }

    public class TaskOrderChangeParams
    {
      public string Method { get; set; }

      public long OrderId { get; set; }

      public Record Data { get; set; }
    }

    [HttpPost("/midos/tasks/change")]
    public INotifyResult<IMessageObject> ChangeTaskOrder([FromBody] TaskOrderChangeParams param)
    {
      var result = NotifyResult.FromVoid();

      _tasks.Change(
        method: param.Method,
        orderId: param.OrderId,
        data: param.Data
      );

      return result.Success("订单状态已修改");
    }

    public class SearchParams: QueryParams
    {
      public long typeId { get; set; }
    }

    [HttpPost("/midos/tasks/orders/search")]
    public IPagination<TaskOrder> Search([FromBody] SearchParams param)
    {
      var query = _domain.Set<TaskOrder>().AsQueryable();

      if (param.typeId != 0) {
        query = query.Where(order => order.TypeId == param.typeId);
      }

      if (param.Query != "") {
        query = query.Where(order => order._data.Contains(param.Query));
      }

      return query
        .OrderByDescending(order => order.CreatedAt)
        .ThenByDescending(order => order.Id)
        .Paginate(param);
    }

    //

    [CapSubscribe(TaskOrderCreate.Message, Group = Group)]
    public void HandleTaskOrderCreate(TaskOrderCreate param)
    {
      var type = _domain.TaskTypes.First(type => type.Key == param.Key);
      var order = TaskOrder.From(type, param);

      _domain.Add(order);

      _domain.SaveChanges(() => {
        _cap.Publish(
          name: TaskOrderChanged.Created,
          contentObj: TaskOrderChanged.From(type, order)
        );
      });
    }

    [CapSubscribe(SubtaskOrderCreate.Message, Group = Group)]
    public void HandleSubtaskOrderCreate(SubtaskOrderCreate param)
    {
      var order = _domain.TaskOrders.Find(param.OrderId);
      var type = _domain.TaskTypes.Find(order.TypeId);
      var subtype = _domain.SubtaskTypes
        .Include(st => st.Subtype)
        .First(st => st.TypeId == order.TypeId && st.Key == param.Subkey);
      var suborder = TaskOrder.From(subtype.Subtype, param);

      _domain.Add(SubtaskOrder.From(subtype, order, suborder));

      _domain.SaveChanges(() => {
        _cap.Publish(
          name: TaskOrderChanged.Created,
          contentObj: TaskOrderChanged.From(subtype.Subtype, suborder)
        );
      });
    }

    [CapSubscribe(TaskOrderChanged.Created, Group = Group)]
    public void HandleTaskOrderCreated(TaskOrderChanged param)
    {
      _cap.Publish(
        name: TaskOrderChange.Start,
        contentObj: TaskOrderChange.From(param.OrderId, new Record())
      );
    }

    [CapSubscribe(TaskOrderChange.Update, Group = Group)]
    public void HandleTaskOrderUpdate(TaskOrderChange param)
    {
      var order = _domain.TaskOrders.Find(param.OrderId);

      order.UseData(param.Data);
      _domain.SaveChanges();
    }

    //

    private void PubilshTaskStatus(
      long orderId,
      Action<TaskOrder> useOrder,
      string message
    ) {
      var order = _domain.TaskOrders.Find(orderId);
      var type = _domain.TaskTypes.Find(order.TypeId);

      useOrder(order);

      _domain.SaveChanges(() => {
        _cap.Publish(
          name: message,
          contentObj: TaskOrderChanged.From(type, order)
        );
      });
    }

    //

    [CapSubscribe(TaskOrderChange.Start, Group = Group)]
    public void HandleTaskOrderStart(TaskOrderChange param)
    {
      PubilshTaskStatus(
        orderId: param.OrderId,
        useOrder: order => order.Start(param.Data),
        message: TaskOrderChanged.Started
      );
    }

    [CapSubscribe(TaskOrderChange.Finish, Group = Group)]
    public void HandleTaskOrderFinish(TaskOrderChange param)
    {
      PubilshTaskStatus(
        orderId: param.OrderId,
        useOrder: order => order.Finish(param.Data),
        message: TaskOrderChanged.Finished
      );
    }

    [CapSubscribe(TaskOrderChange.Cancel, Group = Group)]
    public void HandleTaskOrderCancell(TaskOrderChange param)
    {
      PubilshTaskStatus(
        orderId: param.OrderId,
        useOrder: order => order.Cancel(param.Data),
        message: TaskOrderChanged.Cancelled
      );
    }
  }
}
