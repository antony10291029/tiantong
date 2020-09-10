namespace Namei.Wcs.Api
{
  public class LifterTaskExportedEvent
  {
    public const string Message = "lifter.task.exported";

    public string LifterId { get; set; }

    public string Floor { get; set; }

    public string TaskId { get; set; }

    public LifterTaskExportedEvent(string lifterId, string floor,  string taskId = null)
    {
      Floor = floor;
      LifterId = lifterId;
      TaskId = taskId;
    }
  }
}
