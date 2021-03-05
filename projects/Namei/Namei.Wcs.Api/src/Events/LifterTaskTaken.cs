namespace Namei.Wcs.Api
{
  public class LifterTaskTaken
  {
    public const string Message = "lifter.task.taken";

    public string Barcode { get; init; }

    public string LifterId { get; init; }

    public string Floor { get; init; }

    private LifterTaskTaken() {}

    public static LifterTaskTaken From(
      string barcode,
      string lifterId,
      string floor
    ) {
      return new LifterTaskTaken() {
        Barcode = barcode,
        LifterId = lifterId,
        Floor = floor
      };
    }
  }
}
