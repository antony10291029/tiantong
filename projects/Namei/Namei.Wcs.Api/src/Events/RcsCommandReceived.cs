namespace Namei.Wcs.Api
{
  public class RcsCommandReceivedEvent
  {
    public const string Message = "rcs.command.received";

    public string DoorId { get; set; }

    public string Uuid { get; set; }

    public string Action { get; set; }

    public RcsCommandReceivedEvent(string doorId, string uuid, string action)
    {
      DoorId = doorId;
      Uuid = uuid;
      Action = action;
    }
  }
}
