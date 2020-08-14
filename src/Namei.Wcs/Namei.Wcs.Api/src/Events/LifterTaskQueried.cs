namespace Namei.Wcs.Api
{
  public class LifterTaskQueriedEvent
  {
    public const string Message = "lifter.task.queried";

    public string Floor { get; set;  }

    public string LifterId { get; set; }

    public string Barcode { get; set; }

    public string Destination { get; set; }

    public LifterTaskQueriedEvent(string lifterId, string floor, string barcode, string destination)
    {
      Floor = floor;
      Barcode = barcode;
      LifterId = lifterId;
      Destination = destination;
    }
  }
}
