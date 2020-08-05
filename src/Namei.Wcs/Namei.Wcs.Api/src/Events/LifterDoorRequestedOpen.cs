namespace Namei.Wcs.Api
{
  public struct LifterDoorRequestedOpenEvent
  {
    public const string Message = "lifter.door.requested.open";

    public readonly string LifterId;

    public readonly string Floor;

    public LifterDoorRequestedOpenEvent(string lifterId, string floor)
    {
      LifterId = lifterId;
      Floor = floor;
    }
  }
}
