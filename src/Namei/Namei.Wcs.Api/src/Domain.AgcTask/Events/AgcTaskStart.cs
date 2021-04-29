using Midos.Domain;

namespace Namei.Wcs.Aggregates
{
  public record AgcTaskStart: DomainEvent
  {
    public const string @event = "agc.tasks.start";

    public long Id { get; init; }

    public static AgcTaskStart From(
      long id
    ) => new() {
      Id = id
    };
  }
}
