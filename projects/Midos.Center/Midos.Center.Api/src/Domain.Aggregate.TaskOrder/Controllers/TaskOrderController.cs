using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Midos.Center.Aggregates;
using Midos.Center.Events;
using Midos.Domain;
using System;
using System.Linq;

namespace Midos.Center.Controllers
{
  public class TaskOrderController: BaseController
  {
    private const string Group = "midos.tas.tasks";

    private DomainContext _domain;

    public TaskOrderController(
      DomainContext domain
    ) {
      _domain = domain;
    }

    [HttpPost("/midos/tasks/create")]
    public INotifyResult<IMessageObject> HandleTaskOrderCreate([FromBody] TaskOrderCreate param)
    {
      var result = NotifyResult.FromVoid();

      HandleTaskOrderCreate(TaskOrderCreate.From(
        key: param.Key,
        data: param.Data
      ));

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

      _domain.Publish(param.Method, TaskOrderChange.From(
        orderId: param.OrderId,
        data: param.Data
      ));

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
    public void CreateTaskOrder(TaskOrderCreate param)
    {
      var type = _domain.First<TaskType>(type => type.Key == param.Key);
      var order = TaskOrder.From(type, param);

      _domain.Add(order);

      _domain.SaveChanges(() => {
        _domain.Publish(
          name: TaskOrderChanged.Created,
          data: TaskOrderChanged.From(type, order)
        );
      });
    }

    [CapSubscribe(SubtaskOrderCreate.Message, Group = Group)]
    public void HandleSubtaskOrderCreate(SubtaskOrderCreate param)
    {
      var order = _domain.Find<TaskOrder>(param.OrderId);
      var type = _domain.Find<TaskType>(order.TypeId);
      var subtype = _domain.Set<SubtaskType>()
        .Include(st => st.Subtype)
        .First(st => st.TypeId == order.TypeId && st.Key == param.Subkey);
      var suborder = TaskOrder.From(subtype.Subtype, param);

      _domain.Add(SubtaskOrder.From(subtype, order, suborder));

      _domain.SaveChanges(() => {
        _domain.Publish(
          name: TaskOrderChanged.Created,
          data: TaskOrderChanged.From(subtype.Subtype, suborder)
        );
      });
    }

    [CapSubscribe(TaskOrderChanged.Created, Group = Group)]
    public void CreateTaskOrderd(TaskOrderChanged param)
    {
      _domain.Publish(
        name: TaskOrderChange.Start,
        data: TaskOrderChange.From(param.OrderId, new Record())
      );
    }

    [CapSubscribe(TaskOrderChange.Update, Group = Group)]
    public void HandleTaskOrderUpdate(TaskOrderChange param)
    {
      var order = _domain.Find<TaskOrder>(param.OrderId);

      order.UseData(param.Data);
      _domain.SaveChanges();
    }

    //

    private void PubilshTaskStatus(
      long orderId,
      Action<TaskOrder> useOrder,
      string message
    ) {
      var order = _domain.Find<TaskOrder>(orderId);
      var type = _domain.Find<TaskType>(order.TypeId);

      useOrder(order);

      _domain.SaveChanges(() => {
        _domain.Publish(
          name: message,
          data: TaskOrderChanged.From(type, order)
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
