namespace Namei.Wcs.Api
{
  public class LifterTaskStartedEvent
  {
    public const string Message = "lifter.task.started";

    public int LifterId { get; set; }

    public string Floor { get; set; }

    public LifterTaskStartedEvent(int lifterId, string floor)
    {
      LifterId = lifterId;
      Floor = floor;
    }
  }
}
