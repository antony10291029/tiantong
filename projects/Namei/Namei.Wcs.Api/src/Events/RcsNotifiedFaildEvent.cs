namespace Namei.Wcs.Api
{
  public class RcsNotifiedFailedEvent
  {
    public const string Message = "rcs.notified.failed";

    public string DoorId;

    public string Uuid;

    public string Action;

    public string Result;

    public RcsNotifiedFailedEvent(string doorId, string uuid, string action, string result)
    {
      DoorId = doorId;
      Uuid = uuid;
      Action = action;
      Result = result;
    }
  }
}