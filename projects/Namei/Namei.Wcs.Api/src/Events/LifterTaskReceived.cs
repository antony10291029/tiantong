namespace Namei.Wcs.Api
{
  public class LifterTaskReceived
  {
    public const string Message = "lifter.task.received";

    public string LifterId { get; set; }

    public string Floor { get; set; }

    public string TaskCode { get; set; }

    public string Barcode { get; set; }

    public string Destination { get; set; }

    private LifterTaskReceived() {}

    public static LifterTaskReceived From(
      string lifterId,
      string floor,
      string taskCode = null,
      string barcode = null,
      string destination = null
    ) => new LifterTaskReceived {
      LifterId = lifterId,
      Floor = floor,
      TaskCode = taskCode,
      Barcode = barcode,
      Destination = destination
    };
  }
}
