namespace Namei.Wcs.Api
{
  public class LifterTaskScannedEvent
  {
    public const string Message = "lifter.task.scanned";

    public int LifterId { get; set; }

    public string Floor { get; set; }

    public LifterTaskScannedEvent(int lifterId, string floor)
    {
      LifterId = lifterId;
      Floor = floor;
    }
  }
}
