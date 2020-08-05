namespace Namei.Wcs.Api
{
  public class LifterTaskStartedEvent
  {
    public const string Message = "lifter.task.started";

    public string LifterId { get; set; }

    public string Floor { get; set; }

    public string Destination { get; set; }

    public LifterTaskStartedEvent(string lifterId, string floor, string destination)
    {
      LifterId = lifterId;
      Floor = floor;
      Destination = destination;
    }
  }
}
