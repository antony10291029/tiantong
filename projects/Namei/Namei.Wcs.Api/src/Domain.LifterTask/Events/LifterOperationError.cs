namespace Namei.Wcs.Api
{
  public class LifterOperationError
  {
    public const string Message = "lifter.error";

    public string LifterId { get; init; }

    public string Floor { get; init; }

    public string Operation { get; init; }

    public string Error { get; init; }

    public string Level { get; init; }

    public static LifterOperationError From(
      string lifterId,
      string operation,
      string floor,
      string message,
      string level = "info"
    ) {
      return new LifterOperationError() {
        LifterId = lifterId,
        Floor = floor,
        Error = message,
        Level = level,
      };
    }
  }
}
