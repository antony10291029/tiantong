namespace Namei.Wcs.Api
{
  public class LifterTaskImportedEvent
  {
    public const string Message = "lifter.task.imported";

    public string LifterId { get; set; }

    public string Floor { get; set; }

    public LifterTaskImportedEvent(string lifterId, string floor)
    {
      LifterId = lifterId;
      Floor = floor;
    }
  }
}
