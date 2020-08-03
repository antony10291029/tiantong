namespace Namei.Wcs.Api
{
  public class LifterTaskTakenEvent
  {
    public const string Message = "lifter.task.taken";

    public int LifterId { get; set; }

    public string Floor { get; set; }

    public LifterTaskTakenEvent(int lifterId, string floor)
    {
      LifterId = lifterId;
      Floor = floor;
    }
  }
}
