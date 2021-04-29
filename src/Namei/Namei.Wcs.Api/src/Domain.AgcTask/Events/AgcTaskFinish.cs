using Midos.Domain;

namespace Namei.Wcs.Aggregates
{
  public record AgcTaskFinish: DomainEvent
  {
    public const string @event = "agc.tasks.finish";

    public long Id { get; init; }

    public string AgcCode { get; init; }

    public static AgcTaskFinish From(long id, string agcCode)
      => new() {
        Id = id,
        AgcCode = agcCode,
      };

  }
}
