namespace Namei.Wcs.Api
{
  public class DoorTaskRequestOpenEvent
  {
    public const string Message = "door.task.request.open";

    public string DoorId { get; set; }

    public string TaskId { get; set; }

    public DoorTaskRequestOpenEvent(string id, string taskId)
    {
      DoorId = id;
      TaskId = taskId;
    }
  }
}
