namespace Namei.Wcs.Aggregates
{
  public class RcsAgcTaskCreated
  {
    public const string Message = "rcs.agc.tasks.created";

    public long Id { get; init; }

    public static RcsAgcTaskCreated From(long id)
      => new RcsAgcTaskCreated {
        Id = id,
      };

  }
}
