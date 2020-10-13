namespace Namei.Wcs.Api
{
  public class LifterTaskImportedEvent
  {
    public const string Message = "lifter.task.imported";

    public string LifterId { get; set; }

    public string Floor { get; set; }

    public string TaskId { get; set; }

    public bool IsFromWms { get; set; }

    public LifterTaskImportedEvent(string lifterId, string floor, string taskId = null, bool isFromWms = false)
    {
      LifterId = lifterId;
      Floor = floor;
      TaskId = taskId;
      IsFromWms = isFromWms;
    }
  }
}
