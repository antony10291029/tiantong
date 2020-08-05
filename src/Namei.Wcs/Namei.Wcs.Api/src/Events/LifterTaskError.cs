namespace Namei.Wcs.Api
{
  public class LifterTaskErrorEvent
  {
    public const string Message = "lifter.task.error";

    public string LifterId { get; set; }

    public string Floor { get; set; }

    public LifterTaskErrorEvent(string lifterId, string floor)
    {
      LifterId = lifterId;
      Floor = floor;
    }
  }
}
