namespace Namei.Wcs.Api
{
  public class LifterTaskCreated
  {
    public const string Message = "lifter.task.created";

    public int TaskId { get; init; }

    private LifterTaskCreated()
    {

    }

    public static LifterTaskCreated From(LifterTask task)
    {
      return new LifterTaskCreated() {
        TaskId = task.Id
      };
    }
  }
}
