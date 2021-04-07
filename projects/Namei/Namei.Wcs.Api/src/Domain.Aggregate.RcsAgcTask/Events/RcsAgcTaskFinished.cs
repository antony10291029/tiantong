namespace Namei.Wcs.Aggregates
{
  public class RcsAgcTaskFinished
  {
    public const string Message = "rcs.agc.tasks.finished";

    public long Id { get; init; }

    public string AgcCode { get; init; }

    public static RcsAgcTaskFinished From(long id, string agcCode)
      => new RcsAgcTaskFinished {
        Id = id,
        AgcCode = agcCode,
      };

  }
}
