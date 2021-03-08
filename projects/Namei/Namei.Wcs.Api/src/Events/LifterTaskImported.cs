namespace Namei.Wcs.Api
{
  public class LifterTaskImported
  {
    public const string Message = "lifter.task.imported";

    public string LifterId { get; init; }

    public string Floor { get; init; }

    public string TaskCode { get; init; }

    public string Barcode { get; init; }

    public string Destination { get; init; }

    private LifterTaskImported() {}

    public static LifterTaskImported From(
      string lifterId,
      string floor,
      string taskCode = null,
      string barcode = null,
      string destination = null
    ) => new LifterTaskImported {
      LifterId = lifterId,
      Floor = floor,
      TaskCode = taskCode,
      Barcode = barcode,
      Destination = destination
    };
  }
}
