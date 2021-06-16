using Midos.Domain;

namespace Namei.Wcs.Api
{
  public record LifterTaskFinished: DomainEvent
  {
    public const string Message = "lifter.task.taken";

    public string LifterId { get; init; }

    public string Floor { get; init; }

    public static LifterTaskFinished From(string floor, string lifterId)
      => new() {
        Floor = floor,
        LifterId = lifterId
      };
  }
}
