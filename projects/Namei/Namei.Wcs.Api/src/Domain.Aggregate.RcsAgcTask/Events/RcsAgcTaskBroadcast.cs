using DotNetCore.CAP;

namespace Namei.Wcs.Aggregates
{
  public class RcsAgcTaskOrderFinished
  {
    public static string Message(string orderType)
      => $"rcs.agc.tasks.{orderType}.finished";

    public long OrderId { get; init; }

    public string AgcCode { get; init; }

    public string PodCode { get; init; }

    public static RcsAgcTaskOrderFinished From(
      long orderId,
      string agcCode,
      string podCode
    ) => new RcsAgcTaskOrderFinished {
      OrderId = orderId,
      AgcCode = agcCode,
      PodCode = podCode,
    };
  }
}

namespace Namei.Wcs.Aggregates.Utils
{
  public class RcsAgcTaskOrderFinished: Aggregates.RcsAgcTaskOrderFinished
  {

  }

  public class RcsAgcTaskOrderFinishedAttribute: CapSubscribeAttribute
  {
    public RcsAgcTaskOrderFinishedAttribute(
      string orderType
    ): base($"rcs.agc.tasks.{orderType}.finished") {

    }
  }
}
