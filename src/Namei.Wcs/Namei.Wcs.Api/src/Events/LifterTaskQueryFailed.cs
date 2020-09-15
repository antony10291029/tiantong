namespace Namei.Wcs.Api
{
  public class LifterTaskQueryFailedEvent
  {
    public const string Message = "lifter.task.query.failed";

    public string LifterId { get; set; }

    public string Floor { get; set; }

    public string Barcode { get; set; }

    public LifterTaskQueryFailedEvent(string lifterId, string floor, string barcode)
    {
      Barcode = barcode;
      Floor = floor;
      LifterId = lifterId;
    }
  }
}
