using Midos.Domain;

namespace Namei.Wcs.Api
{
  public record WcsDoorEvent: DomainEvent
  {
    public const string Open = "wcs.door.open";

    public const string Opened = "wcs.door.opened";

    public const string Close = "wcs.door.close";

    public const string Closed = "wcs.door.closed";

    public string DoorId { get; init; }

    public static WcsDoorEvent From(string doorId)
      => new WcsDoorEvent { DoorId = doorId };

    public static WcsDoorEvent From(RcsDoorEvent param)
      => From(param.DoorId);
  }
}
