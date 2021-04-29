using Midos.Domain;
using System;

namespace Namei.Wcs.Aggregates
{
  public class AgcTask: IEntity, IAggregateRoot
  {
    public long Id { get; private set; }

    public long TypeId { get; private set; }

    public string Position { get; private set; }

    public string Destination { get; private set; }

    public string PodCode { get; private set; }

    public string AgcCode { get; private set; }

    public string Priority { get; private set; }

    public string TaskId { get; private set; }

    public string RcsTaskCode { get; private set; }

    public string Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime StartedAt { get; private set; }

    public DateTime ClosedAt { get; private set; }

    public AgcTaskType Type { get; private set; }

    public static AgcTask From(
      long typeId,
      string position,
      string destination,
      string podCode = "",
      string taskId = "",
      string priority = "",
      string agcCode = ""
    ) => new() {
      TypeId = typeId,
      Position = position,
      Destination = destination,
      PodCode = podCode,
      Priority = priority,
      RcsTaskCode = "",
      AgcCode = agcCode,
      Status = AgcTaskStatus.Created,
      TaskId = taskId,
      CreatedAt = DateTime.MinValue,
      StartedAt = DateTime.MinValue,
      ClosedAt = DateTime.MinValue,
    };

    public static AgcTask From(AgcTaskCreate param)
      => From(
        typeId: param.TypeId,
        position: param.Position,
        destination: param.Destination,
        podCode: param.PodCode,
        priority: param.Priority,
        taskId: param.TaskId
      );

    public void Start(string taskCode)
    {
      RcsTaskCode = taskCode;
      StartedAt = DateTime.Now;
      Status = AgcTaskStatus.Started;
    }

    public void Finish(string agcCode)
    {
      AgcCode = agcCode;
      ClosedAt = DateTime.Now;
      Status = AgcTaskStatus.Finished;
    }

    public void Close()
    {
      ClosedAt = DateTime.Now;
      Status = AgcTaskStatus.Closed;
    }

  }
}
