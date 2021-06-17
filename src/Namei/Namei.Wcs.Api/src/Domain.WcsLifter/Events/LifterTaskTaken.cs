using Midos.Domain;

namespace Namei.Wcs.Api
{
  public record LifterTaskTaken: DomainEvent
  {
    public const string Message = "lifter.task.taken";

    public string LifterId { get; init; }

    public string Floor { get; init; }

    public static LifterTaskTaken From(string floor, string lifterId)
      => new LifterTaskTaken() {
        Floor = floor,
        LifterId = lifterId
      };
  }
}
