namespace Namei.Wcs.Api
{
  public class DoorOpenedEvent
  {
    public const string Message = "door.opened";

    public string DoorId { get; set; }

    public DoorOpenedEvent(string id)
    {
      DoorId = id;
    }
  }
}
