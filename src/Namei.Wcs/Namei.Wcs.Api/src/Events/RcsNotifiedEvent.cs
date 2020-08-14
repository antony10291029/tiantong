namespace Namei.Wcs.Api
{
  public class RcsNotifiedEvent
  {
    public const string Message = "rcs.notified";

    public string DoorId { get; set; }

    public string Action { get; set; }

    public string Uuid { get; set; }

    public string Result  { get; set; }

    public RcsNotifiedEvent(string doorId, string action, string uuid, string result)
    {
      DoorId = doorId;
      Action = action;
      Uuid = uuid;
      Result = result;
    }
  }
}
