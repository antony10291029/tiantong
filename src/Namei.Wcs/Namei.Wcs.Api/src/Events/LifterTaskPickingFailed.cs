namespace Namei.Wcs.Api
{
  public class LifterTaskPickingFailedEvent
  {
    public const string Message = "lifter.task.picking.failed";

    public string Floor { get; set;  }

    public string LifterId { get; set; }

    public string Barcode { get; set; }

    public LifterTaskPickingFailedEvent(string lifterId, string floor, string barcode)
    {
      Floor = floor;
      Barcode = barcode;
      LifterId = lifterId;
    }
  }
}
