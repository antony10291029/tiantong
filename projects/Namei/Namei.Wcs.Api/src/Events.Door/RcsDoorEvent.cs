namespace Namei.Wcs.Api
{
  public class RcsDoorEvent
  {
    public const string Request = "rcs.door.request";

    public const string Requested = "rcs.door.requested";

    public const string Enter = "rcs.door.enter";

    public const string Entered = "rcs.door.entered";

    public const string Leave = "rcs.door.leave";

    public const string Left = "rcs.door.left";

    public string Uuid { get; init; }

    public string DoorId { get; init; }

    private RcsDoorEvent() {}

    public static RcsDoorEvent From(string uuid, string doorId)
      => new RcsDoorEvent {
        Uuid = uuid,
        DoorId = doorId
      };
  }
}
