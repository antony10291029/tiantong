namespace Namei.Wcs.Aggregates
{
  public class RcsAgcTaskClose
  {
    public const string Message = "rcs.agc.tasks.close";

    public long Id { get; init; }

    public static RcsAgcTaskClose From(long id)
      => new RcsAgcTaskClose {
        Id = id,
      };

  }
}
