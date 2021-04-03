using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Midos.Center.Entities;
using Midos.Center.Events;
using Midos.Domain;
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

    [CapSubscribe(TaskOrderChanged.Created, Group = Group)]
    public void HandleTaskOrderCreated(TaskOrderChanged param)
    {
      _cap.Publish(
        name: TaskOrderBroadcast.Created(param.Key),
        contentObj: TaskOrderBroadcast.From(param.OrderId, param.Data)
      );
    }

    [CapSubscribe(TaskOrderChanged.Started, Group = Group)]
    public void HandleTaskOrderStarted(TaskOrderChanged param)
    {
      _cap.Publish(
        name: TaskOrderBroadcast.Started(param.Key),
        contentObj: TaskOrderBroadcast.From(param.OrderId, param.Data)
      );
    }

    [CapSubscribe(TaskOrderChanged.Finished, Group = Group)]
    public void HandleTaskOrderFinished(TaskOrderChanged param)
    {
      _cap.Publish(
        name: TaskOrderBroadcast.Finished(param.Key),
        contentObj: TaskOrderBroadcast.From(param.OrderId, param.Data)
      );
    }

    [CapSubscribe(TaskOrderChanged.Cancelled, Group = Group)]
    public void HandleTaskOrderCancelled(TaskOrderChanged param)
    {
      _cap.Publish(
        name: TaskOrderBroadcast.Cancelled(param.Key),
        contentObj: TaskOrderBroadcast.From(param.OrderId, param.Data)
      );
    }

    //

    private void BroadcastSubtask(
      long suborderId,
      object subdata,
      Func<string, string, string> useStatus
    ) {
      var suborder = _domain.Set<SubtaskOrder>()
        .Include(so => so.Order)
          .ThenInclude(so => so.Type)
        .Include(so => so.Subtype)
        .FirstOrDefault(so => so.SuborderId == suborderId);

      if (suborder != null) {
        var order = suborder.Order;
        var type = order.Type;
        var subtype = suborder.Subtype;

        _cap.Publish(
          name: useStatus(type.Key, subtype.Key),
          contentObj: SubtaskOrderBroadcast.From(order, suborderId, subdata)
        );
      }
    }

    //

    [CapSubscribe(TaskOrderChanged.Started, Group = SubGroup)]
    public void HandleSubtaskOrderStarted(TaskOrderChange param)
    {
      BroadcastSubtask(
        suborderId: param.OrderId,
        subdata: param.Data,
        useStatus: SubtaskOrderBroadcast.Started
      );
    }

    [CapSubscribe(TaskOrderChanged.Finished, Group = SubGroup)]
    public void HandleSubtaskOrderFinished(TaskOrderChanged param)
    {
      BroadcastSubtask(
        suborderId: param.OrderId,
        subdata: param.Data,
        useStatus: SubtaskOrderBroadcast.Finished
      );
    }

    [CapSubscribe(TaskOrderChanged.Cancelled, Group = SubGroup)]
    public void HandleSubtaskOrderCancelled(TaskOrderChanged param)
    {
      BroadcastSubtask(
        suborderId: param.OrderId,
        subdata: param.Data,
        useStatus: SubtaskOrderBroadcast.Cancelled
      );
    }
  }
}
