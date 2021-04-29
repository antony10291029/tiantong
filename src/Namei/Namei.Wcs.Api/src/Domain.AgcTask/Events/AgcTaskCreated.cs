using Midos.Domain;

namespace Namei.Wcs.Aggregates
{
  public record AgcTaskCreated: DomainEvent
  {
    public const string @event = "agc.tasks.created";

    public long Id { get; init; }

    public static AgcTaskCreated From(long id)
      => new() {
        Id = id,
      };
  }
}
