namespace Namei.Wcs.Api
{
  public struct LifterDoorRequestedCloseEvent
  {
    public const string Message = "lifter.door.requested.close";

    public readonly string LifterId;

    public readonly string Floor;

    public LifterDoorRequestedCloseEvent(string lifterId, string floor)
    {
      LifterId = lifterId;
      Floor = floor;
    }
  }
}
