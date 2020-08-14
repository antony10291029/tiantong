namespace Namei.Wcs.Api
{
  public class DoorTaskHandleEvent
  {
    public const string Message = "door.task.handle";

    public string DoorId { get; set; }

    public DoorTaskHandleEvent(string doorId)
    {
      DoorId = doorId;
    }
  }
}
