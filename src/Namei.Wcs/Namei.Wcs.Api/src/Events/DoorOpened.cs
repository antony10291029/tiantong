namespace Namei.Wcs.Api
{
  public class DoorOpenedEvent
  {
    public const string Message = "door.opened";

    public int DoorId { get; set; }

    public DoorOpenedEvent(int id)
    {
      DoorId = id;
    }
  }
}
