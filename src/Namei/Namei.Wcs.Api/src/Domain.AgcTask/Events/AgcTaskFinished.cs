using Midos.Domain;

namespace Namei.Wcs.Aggregates
{
  public record AgcTaskFinished: DomainEvent
  {
    public const string @event = "agc.tasks.finished";

    public long Id { get; init; }

    public static AgcTaskFinished From(long id)
      => new() {
        Id = id,
      };

  }
}
