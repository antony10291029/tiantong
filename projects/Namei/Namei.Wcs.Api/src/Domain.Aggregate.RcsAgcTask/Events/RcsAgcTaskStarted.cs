namespace Namei.Wcs.Aggregates
{
  public class RcsAgcTaskStarted
  {
    public const string Message = "rcs.agc.tasks.started";

    public long Id { get; init; }

    public string TaskCode { get; init; }

    public static RcsAgcTaskStarted From(long id, string taskCode)
      => new RcsAgcTaskStarted {
        Id = id,
        TaskCode = taskCode,
      };

  }
}
