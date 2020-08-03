namespace Namei.Wcs.Api
{
  public class DoorRequestingCloseEvent
  {
    public const string Message = "door.requesting.close";

    public int DoorId { get; set; }

    public DoorRequestingCloseEvent(int id)
    {
      DoorId = id;
    }
  }
}
