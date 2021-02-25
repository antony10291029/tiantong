namespace Namei.Wcs.Api
{
  public class LifterTaskImportedEvent
  {
    public const string Message = "lifter.task.imported";

    public string LifterId { get; set; }

    public string Floor { get; set; }

    public string TaskCode { get; set; }

    public string BarCode { get; set; }

    public string Destination { get; set; }

    public bool IsFromWms { get => TaskCode != null; }

    public LifterTaskImportedEvent(string lifterId, string floor, string taskCode = null, string barCode = null, string destination = null)
    {
      LifterId = lifterId;
      Floor = floor;
      TaskCode = taskCode;
      BarCode = barCode;
      Destination = destination;
    }
  }
}
