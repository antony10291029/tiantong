namespace Namei.Wcs.Api
{
  public class LifterTaskPickingEvent
  {
    public const string Message = "lifter.task.picking";

    public string Floor { get; set;  }

    public string LifterId { get; set; }

    public string Barcode { get; set; }

    public LifterTaskPickingEvent(string lifterId, string floor, string barcode)
    {
      Floor = floor;
      Barcode = barcode;
      LifterId = lifterId;
    }
  }
}
