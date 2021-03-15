using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Midos.Center.Events;
using System;
using System.Linq;

namespace Midos.Center.Controllers
{
  public class TaskOrderBroadcastController: BaseController
  {
    private const string Group = "midos.tas.boardcast";

    private const string SubGroup = "midos.tas.broadcast.suborder";

    private ICapPublisher _cap;

    private DomainContext _domain;

    public TaskOrderBroadcastController(
      ICapPublisher cap,
      DomainContext domain
    ) {
      _cap = cap;
      _domain = domain;
    }

    [CapSubscribe(TaskOrderCreated.Message, Group = Group)]
    public void HandleTaskOrderCreated(TaskOrderCreated param)
    {
      _cap.Publish(
        name: TaskOrderBroadcast.Created(param.Key),
        contentObj: TaskOrderBroadcast.From(param.OrderId, param.Data)
      );
    }

    [CapSubscribe(TaskOrderStarted.Message, Group = Group)]
    public void HandleTaskOrderStarted(TaskOrderStarted param)
    {
      _cap.Publish(
        name: TaskOrderBroadcast.Started(param.Key),
        contentObj: TaskOrderBroadcast.From(param.OrderId, param.Data)
      );
    }

    [CapSubscribe(TaskOrderFinished.Message, Group = Group)]
    public void HandleTaskOrderFinished(TaskOrderFinish param)
    {
      _cap.Publish(
        name: TaskOrderBroadcast.Finished(param.Key),
        contentObj: TaskOrderBroadcast.From(param.OrderId, param.Data)
      );
    }

    [CapSubscribe(TaskOrderCancelled.Message, Group = Group)]
    public void HandleTaskOrderCancelled(TaskOrderCancelled param)
    {
      _cap.Publish(
        name: TaskOrderBroadcast.Cancelled(param.Key),
        contentObj: TaskOrderBroadcast.From(param.OrderId, param.Data)
      );
    }

    [CapSubscribe(SubtaskOrderCreated.Message, Group = Group)]
    public void HandleSubtaskOrderCreated(SubtaskOrderCreated param)
    {
      _cap.Publish(
        name: SubtaskOrderBroadcast.Created(param.Key, param.Subkey),
        contentObj: SubtaskOrderBroadcast.From(param.OrderId, param.SuborderId, param.Data)
      );
    }

    private void BroadcastSubtask(
      string key,
      long orderId,
      string data,
      Func<string, string, string> useStatus
    ) {
      var suborder = _domain.SubtaskOrders
        .FirstOrDefault(so => so.SuborderId == orderId);

      if (suborder != null) {
        var subtype = _domain.SubtaskTypes.Find(suborder.SubtypeId);

        _cap.Publish(
          name: useStatus(key, subtype.Key),
          contentObj: SubtaskOrderBroadcast.From(suborder, data)
        );
      }
    }

    [CapSubscribe(TaskOrderStarted.Message, Group = SubGroup)]
    public void HandleSubtaskOrderStarted(TaskOrderStarted param)
    {
      BroadcastSubtask(
        key: param.Key,
        orderId: param.OrderId,
        data: param.Data,
        useStatus: SubtaskOrderBroadcast.Started
      );
    }

    [CapSubscribe(TaskOrderFinished.Message, Group = SubGroup)]
    public void HandleSubtaskOrderFinished(TaskOrderFinished param)
    {
      BroadcastSubtask(
        key: param.Key,
        orderId: param.OrderId,
        data: param.Data,
        useStatus: SubtaskOrderBroadcast.Finished
      );
    }

    [CapSubscribe(TaskOrderCancelled.Message, Group = SubGroup)]
    public void HandleSubtaskOrderCancelled(TaskOrderCancelled param)
    {
      BroadcastSubtask(
        key: param.Key,
        orderId: param.OrderId,
        data: param.Data,
        useStatus: SubtaskOrderBroadcast.Cancelled
      );
    }
  }
}
