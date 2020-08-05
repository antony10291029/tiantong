namespace Namei.Wcs.Api
{
  public class DoorRequestedOpenEvent
  {
    public const string Message = "door.requested.open";

    public string DoorId { get; set; }

    public DoorRequestedOpenEvent(string id)
    {
      DoorId = id;
    }
  }
}
