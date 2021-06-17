namespace Namei.Wcs.Api
{
  public class LifterTaskScannedEvent
  {
    public const string Message = "lifter.task.scanned";

    public string LifterId { get; set; }

    public string Floor { get; set; }

    public LifterTaskScannedEvent(string lifterId, string floor)
    {
      LifterId = lifterId;
      Floor = floor;
    }
  }
}
