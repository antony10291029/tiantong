using Midos.Domain;

namespace Namei.Wcs.Aggregates
{
  public record RcsAgcTaskCreate: DomainEvent
  {
    public const string Message = "rcs.agc.tasks.create";

    public string TaskType { get; init; }

    public string Position { get; init; }

    public string Destination { get; init; }

    public string Priority { get; init; }

    public string PodCode { get; init; }

    public string Comment { get; init; }

    public string OrderType { get; init; }

    public long OrderId { get; init; }

    public static RcsAgcTaskCreate From(
      string taskType,
      string position,
      string destination,
      string priority = "",
      string podCode = "",
      string comment = "",
      string orderType = "",
      long orderId = 0
    ) => new RcsAgcTaskCreate {
      TaskType = taskType,
      Position = position,
      Destination = destination,
      Priority = priority,
      PodCode = podCode,
      Comment = comment,
      OrderType = orderType,
      OrderId = orderId
    };

  }
}
