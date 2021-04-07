namespace Namei.Wcs.Aggregates
{
  public class RcsAgcTaskStart
  {
    public const string Message = "rcs.agc.tasks.start";

    public long Id { get; init; }

    public bool IsEnforced { get; init; }

    public static RcsAgcTaskStart From(
      long id,
      bool isEnforced = false
    ) => new RcsAgcTaskStart {
      Id = id,
      IsEnforced = isEnforced
    };

    public static RcsAgcTaskStart From(RcsAgcTaskCreated param)
      => From(
        id: param.Id,
        isEnforced: false
      );

  }
}
