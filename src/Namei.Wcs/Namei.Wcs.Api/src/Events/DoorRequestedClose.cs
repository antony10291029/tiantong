namespace Namei.Wcs.Api
{
  public class DoorRequestedCloseEvent
  {
    public const string Message = "door.requested.close";

    public string DoorId { get; set; }

    public DoorRequestedCloseEvent(string id)
    {
      DoorId = id;
    }
  }
}
