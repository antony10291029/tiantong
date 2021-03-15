using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Midos.Center.Entities;
using Midos.Center.Events;
using System;
using System.Linq;

namespace Midos.Center.Controllers
{
  public class TaskOrderInternalController: BaseController
  {
    private const string Group = "midos.tas.tasks";

    private ICapPublisher _cap;

    private DomainContext _domain;

    public TaskOrderInternalController(
      DomainContext domain,
      ICapPublisher cap
    ) {
      _cap = cap;
      _domain = domain;
    }

    [CapSubscribe(TaskOrderCreate.Message, Group = Group)]
    public void HandleTaskOrderCreate(TaskOrderCreate param)
    {
      var type = _domain.TaskTypes.First(type => type.Key == param.Key);
      var order = TaskOrder.From(type, param);

      _domain.Add(order);

      _domain.SaveChanges(() => {
        _cap.Publish(
          name: TaskOrderCreated.Message,
          contentObj: TaskOrderCreated.From(type, order)
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
          name: SubtaskOrderCreated.Message,
          contentObj: SubtaskOrderCreated.From(type, subtype, order, suborder)
        );        
      });
    }

    [CapSubscribe(TaskOrderCreated.Message, Group = Group)]
    public void HandleSubtaskOrderCreated(TaskOrderCreated param)
    {
      _cap.Publish(
        name: TaskOrderStart.Message,
        contentObj: TaskOrderStart.From(param.OrderId, param.Data)
      );
    }

    [CapSubscribe(SubtaskOrderCreated.Message, Group = Group)]
    public void HandleSubtaskOrderCreated(SubtaskOrderCreated param)
    {
      _cap.Publish(
        name: TaskOrderCreated.Message,
        contentObj: TaskOrderCreated.From(param)
      );
    }

    private void PubilshTaskStatus(
      long orderId,
      Action<TaskOrder> useOrder,
      string message,
      Func<TaskType, TaskOrder, object> useEvent
    ) {
      var order = _domain.TaskOrders.Find(orderId);
      var type = _domain.TaskTypes.Find(order.TypeId);

      useOrder(order);

      _domain.SaveChanges(() => {
        _cap.Publish(
          name: message,
          contentObj: useEvent(type, order)
        );
      });
    }

    [CapSubscribe(TaskOrderStart.Message, Group = Group)]
    public void HandleTaskOrderStart(TaskOrderStart param)
    {
      PubilshTaskStatus(
        orderId: param.OrderId,
        useOrder: order => order.Start(param.Data),
        message: TaskOrderStarted.Message,
        useEvent: TaskOrderStarted.From
      );
    }

    [CapSubscribe(TaskOrderFinish.Message, Group = Group)]
    public void HandleTaskOrderFinish(TaskOrderFinish param)
    {
      PubilshTaskStatus(
        orderId: param.OrderId,
        useOrder: order => order.Finish(param.Data),
        message: TaskOrderFinished.Message,
        useEvent: TaskOrderFinished.From
      );
    }

    [CapSubscribe(TaskOrderCancell.Message, Group = Group)]
    public void HandleTaskOrderCancell(TaskOrderCancell param)
    {
      PubilshTaskStatus(
        orderId: param.OrderId,
        useOrder: order => order.Cancell(param.Data),
        message: TaskOrderCancelled.Message,
        useEvent: TaskOrderCancelled.From
      );
    }
  }
}
