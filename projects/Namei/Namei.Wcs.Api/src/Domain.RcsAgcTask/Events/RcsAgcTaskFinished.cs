using Midos.Domain;

namespace Namei.Wcs.Aggregates
{
  public record RcsAgcTaskFinished: DomainEvent
  {
    public const string Message = "rcs.agc.tasks.finished";

    public long Id { get; init; }

    public static RcsAgcTaskFinished From(long id)
      => new RcsAgcTaskFinished {
        Id = id,
      };

  }
}
