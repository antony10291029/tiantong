namespace Namei.Wcs.Api
{
  public class LifterTaskCodeReaded
  {
    public const string Message = "lifter.task.code.readed";

    public string LifterId { get; set; }

    public string Floor { get; set; }

    public LifterTaskCodeReaded(string lifterId, string floor)
    {
      LifterId = lifterId;
      Floor = floor;
    }
  }
}
