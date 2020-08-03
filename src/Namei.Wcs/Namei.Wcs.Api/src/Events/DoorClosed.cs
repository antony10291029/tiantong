namespace Namei.Wcs.Api
{
  public class DoorClosedEvent
  {
    public const string Message = "door.closed";

    public int DoorId { get; set; }

    public DoorClosedEvent(int id)
    {
      DoorId = id;
    }
  }
}
