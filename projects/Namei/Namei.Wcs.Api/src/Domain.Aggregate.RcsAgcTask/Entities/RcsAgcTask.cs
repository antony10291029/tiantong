using Midos.Domain;
using System;

namespace Namei.Wcs.Aggregates
{
  public class RcsAgcTask: IEntity, IAggregateRoot
  {
    public long Id { get; private set; }

    public string Position { get; private set; }

    public string Destination { get; private set; }

    public string Comment { get; private set; }

    public string TaskType { get; private set; }

    public string TaskCode { get; private set; }

    public string OrderType { get; private set; }

    public long OrderId { get; private set; }

    public string AgcCode { get; private set; }

    public string PodCode { get; private set; }

    public string Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime StartedAt { get; private set; }

    public DateTime ClosedAt { get; private set; }

    public static RcsAgcTask From(
      string taskType,
      string position,
      string destination,
      string podCode = "",
      string comment = "",
      string orderType = "",
      long orderId = 0
    ) => new RcsAgcTask {
      TaskType = taskType,
      Position = position,
      Destination = destination,
      PodCode = podCode,
      TaskCode = "",
      AgcCode = "",
      Comment = comment,
      Status = RcsAgcTaskStatus.Created,
      OrderType = orderType,
      OrderId = orderId,
      CreatedAt = DateTime.Now,
      StartedAt = DateTime.Now,
      ClosedAt = DateTime.Now,
    };

    public static RcsAgcTask From(RcsAgcTaskCreate param)
      => From(
        taskType: param.TaskType,
        position: param.Position,
        destination: param.Destination,
        podCode: param.PodCode,
        comment: param.Comment,
        orderType: param.OrderType,
        orderId: param.OrderId
      );

    public void Start(string taskCode)
    {
      TaskCode = taskCode;
      StartedAt = DateTime.Now;
      Status = RcsAgcTaskStatus.Started;
    }

    public void Finish(string agcCode)
    {
      AgcCode = agcCode;
      ClosedAt = DateTime.Now;
      Status = RcsAgcTaskStatus.Finished;
    }

    public void Close()
    {
      ClosedAt = DateTime.Now;
      Status = RcsAgcTaskStatus.Closed;
    }

  }
}
