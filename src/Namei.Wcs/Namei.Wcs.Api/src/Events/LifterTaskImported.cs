namespace Namei.Wcs.Api
{
  public class LifterTaskImportedEvent
  {
    public const string Message = "lifter.task.imported";

    public int LifterId { get; set; }

    public string Floor { get; set; }

    public LifterTaskImportedEvent(int lifterId, string floor)
    {
      LifterId = lifterId;
      Floor = floor;
    }
  }
}
