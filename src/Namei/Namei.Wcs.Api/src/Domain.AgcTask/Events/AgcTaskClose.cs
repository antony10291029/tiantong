using Midos.Domain;

namespace Namei.Wcs.Aggregates
{
  public record AgcTaskClose: DomainEvent
  {
    public const string @event = "agc.tasks.close";

    public long Id { get; init; }

    public static AgcTaskClose From(long id)
      => new() {
        Id = id,
      };
  }
}
