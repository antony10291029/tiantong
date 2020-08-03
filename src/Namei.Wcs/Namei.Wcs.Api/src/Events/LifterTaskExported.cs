namespace Namei.Wcs.Api
{
  public class LifterTaskExportedEvent
  {
    public const string Message = "lifter.task.exported";

    public int LifterId { get; set; }

    public string Floor { get; set; }

    public LifterTaskExportedEvent(int lifterId, string floor)
    {
      Floor = floor;
      LifterId = lifterId;
    }
  }
}
