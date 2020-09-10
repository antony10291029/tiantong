namespace Namei.Wcs.Api
{
  public class LifterTaskTakenEvent
  {
    public const string Message = "lifter.task.taken";

    public string LifterId { get; set; }

    public string Floor { get; set; }

    public string TaskId { get; set; }

    public LifterTaskTakenEvent(string lifterId, string floor, string taskId = null)
    {
      LifterId = lifterId;
      Floor = floor;
      TaskId = taskId;
    }
  }
}
