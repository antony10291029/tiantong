using System;

namespace Namei.Wcs.Api
{
  public class LifterTaskLogger: Logger
  {
    public LifterTaskLogger(DomainContext domain): base(domain) {}

    private void FromTask(LifterTask task, string message, params Action<Log>[] hooks)
    {
      var log = Log.From(hooks);

      Log.Use(
        log,
        Log.UseClass("wcs.lifter"),
        Log.UseMessage($"{task.LifterId} 号梯，{task.Floor} 楼，{message}")
      );

      Save(log);
    }
  }
}
