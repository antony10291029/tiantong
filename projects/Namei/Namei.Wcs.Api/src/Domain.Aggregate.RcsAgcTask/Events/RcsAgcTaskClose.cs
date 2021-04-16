using Midos.Domain;

namespace Namei.Wcs.Aggregates
{
  public record RcsAgcTaskClose: DomainEvent
  {
    public const string Message = "rcs.agc.tasks.close";

    public long Id { get; init; }

    public static RcsAgcTaskClose From(long id)
      => new RcsAgcTaskClose {
        Id = id,
      };

  }
}
