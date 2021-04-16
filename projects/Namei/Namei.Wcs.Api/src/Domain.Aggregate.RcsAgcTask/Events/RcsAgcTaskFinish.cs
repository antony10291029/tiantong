using Midos.Domain;

namespace Namei.Wcs.Aggregates
{
  public record RcsAgcTaskFinish: DomainEvent
  {
    public const string Message = "rcs.agc.tasks.finish";

    public long Id { get; init; }

    public string AgcCode { get; init; }

    public static RcsAgcTaskFinish From(long id, string agcCode)
      => new RcsAgcTaskFinish {
        Id = id,
        AgcCode = agcCode,
      };

  }
}
