namespace Namei.Wcs.Api
{
  public class LifterTaskTaken
  {
    public const string Message = "lifter.task.taken";

    public string LifterId { get; init; }

    public string Floor { get; init; }

    private LifterTaskTaken() {}

    public static LifterTaskTaken From(string floor, string lifterId)
      => new LifterTaskTaken() {
        Floor = floor,
        LifterId = lifterId
      };
  }
}
