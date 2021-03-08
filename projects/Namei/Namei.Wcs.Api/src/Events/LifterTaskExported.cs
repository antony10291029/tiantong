namespace Namei.Wcs.Api
{
  public class LifterTaskExported
  {
    public const string Message = "lifter.task.exported";

    public string LifterId { get; init; }

    public string Floor { get; init; }

    private LifterTaskExported() {}

    public static LifterTaskExported From(string lifterId, string floor)
      => new LifterTaskExported {
        Floor = floor,
        LifterId = lifterId,
      };
  }
}
