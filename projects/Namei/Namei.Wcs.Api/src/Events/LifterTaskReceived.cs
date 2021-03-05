namespace Namei.Wcs.Api
{
  public class LifterTaskReceived
  {
    public const string Message = "lifter.task.received";

    public string LifterId { get; init; }

    public string Floor { get; init; }

    public string TaskCode { get; init; }

    public string Barcode { get; init; }

    public string Destination { get; init; }

    public string Operator { get; init; }

    private LifterTaskReceived()
    {

    }

    public static LifterTaskReceived From(
      string lifterId,
      string floor,
      string destination,
      string barcode,
      string taskCode,
      string operatr
    ) {
      return new LifterTaskReceived() {
        LifterId = lifterId,
        Floor = floor,
        Destination = destination,
        Barcode = barcode,
        TaskCode = taskCode,
        Operator = operatr
      };
    }
  }
}
