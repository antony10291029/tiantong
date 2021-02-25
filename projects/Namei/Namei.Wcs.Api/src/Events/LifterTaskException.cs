namespace Namei.Wcs.Api
{
  public class LifterTaskExceptionEvent
  {
    public const string Message = "lifter.task.picking.failed";

    public string Floor { get; set;  }

    public string LifterId { get; set; }

    public string ErrorMessage { get; set;  }

    public LifterTaskExceptionEvent(string lifterId, string floor, string message)
    {
      Floor = floor;
      LifterId = lifterId;
      ErrorMessage = message;
    }
  }
}
