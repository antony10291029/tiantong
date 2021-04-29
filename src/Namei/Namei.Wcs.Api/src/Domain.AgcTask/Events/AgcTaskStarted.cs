using Midos.Domain;

namespace Namei.Wcs.Aggregates
{
  public record AgcTaskStarted: DomainEvent
  {
    public const string @event = "agc.tasks.started";

    public long Id { get; init; }

    public string TaskCode { get; init; }

    public static AgcTaskStarted From(long id, string taskCode)
      => new() {
        Id = id,
        TaskCode = taskCode,
      };
  }
}
