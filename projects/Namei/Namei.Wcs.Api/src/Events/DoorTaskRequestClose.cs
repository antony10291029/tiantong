namespace Namei.Wcs.Api
{
  public class DoorTaskRequestCloseEvent
  {
    public const string Message = "door.task.request.close";

    public string DoorId { get; set; }

    public string TaskId { get; set; }

    public DoorTaskRequestCloseEvent(string id, string taskId)
    {
      DoorId = id;
      TaskId = taskId;
    }
  }
}
