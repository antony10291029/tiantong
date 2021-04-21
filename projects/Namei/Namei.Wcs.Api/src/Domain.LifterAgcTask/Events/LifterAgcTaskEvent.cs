using Midos.Domain;

namespace Namei.Wcs.Aggregates
{
  public record LifterAgcTaskEvent: DomainEvent
  {
    public const string Created = "lifter.agc.tasks.created";

    public const string Closed = "lifter.agc.tasks.closed";

    public long Id { get; init; }

    public static LifterAgcTaskEvent From(long id)
      => new LifterAgcTaskEvent { Id = id };
  }
}
