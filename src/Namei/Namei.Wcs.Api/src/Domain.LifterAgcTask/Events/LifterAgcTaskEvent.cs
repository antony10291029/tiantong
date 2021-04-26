using System.Runtime.InteropServices;
using Midos.Domain;

namespace Namei.Wcs.Aggregates
{
  public record LifterAgcTaskEvent: DomainEvent
  {
    public const string Created = "lifter.agc.tasks.created";

    public const string Started = "lifter.agc.tasks.started";

    public const string Exported = "lifter.agc.tasks.exported";

    public const string Finished = "lifter.agc.tasks.finished";

    public const string Closed = "lifter.agc.tasks.closed";

    public long Id { get; init; }

    public static LifterAgcTaskEvent From(long id)
      => new LifterAgcTaskEvent { Id = id };
  }
}
