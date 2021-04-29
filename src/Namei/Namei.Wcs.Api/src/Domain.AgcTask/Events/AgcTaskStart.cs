using Midos.Domain;

namespace Namei.Wcs.Aggregates
{
  public record AgcTaskStart: DomainEvent
  {
    public const string @event = "agc.tasks.start";

    public long Id { get; init; }

    public bool IsEnforced { get; init; }

    public static AgcTaskStart From(
      long id,
      bool isEnforced = false
    ) => new() {
      Id = id,
      IsEnforced = isEnforced
    };

    public static AgcTaskStart From(AgcTaskCreated param)
      => From(
        id: param.Id,
        isEnforced: false
      );
  }
}
