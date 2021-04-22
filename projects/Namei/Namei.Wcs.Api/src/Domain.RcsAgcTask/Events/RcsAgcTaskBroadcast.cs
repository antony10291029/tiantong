using DotNetCore.CAP;
using Midos.Domain;

namespace Namei.Wcs.Aggregates
{
  public class RcsAgcTaskOrderFinishedAttribute: CapSubscribeAttribute
  {
    public RcsAgcTaskOrderFinishedAttribute(
      string orderType
    ): base($"rcs.agc.tasks.{orderType}.finished") {

    }
  }

  public record RcsAgcTaskOrderFinished: DomainEvent
  {
    public static string Message(string orderType)
      => $"rcs.agc.tasks.{orderType}.finished";

    public long Id { get; init; }

    public long OrderId { get; init; }

    public string AgcCode { get; init; }

    public string PodCode { get; init; }

    public static RcsAgcTaskOrderFinished From(
      long id,
      long orderId,
      string agcCode,
      string podCode
    ) => new RcsAgcTaskOrderFinished {
      Id = id,
      OrderId = orderId,
      AgcCode = agcCode,
      PodCode = podCode,
    };
  }
}
