namespace Namei.Wcs.Api
{
  public class DoorRequestingOpenEvent
  {
    public const string Message = "door.requesting.open";

    public int DoorId { get; set; }

    public DoorRequestingOpenEvent(int id)
    {
      DoorId = id;
    }
  }
}
