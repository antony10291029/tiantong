namespace Namei.Wcs.Api
{
  public class DoorClosedEvent
  {
    public const string Message = "door.closed";

    public string DoorId { get; set; }

    public DoorClosedEvent(string id)
    {
      DoorId = id;
    }
  }
}
