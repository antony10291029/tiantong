using System;

namespace Namei.Wcs.Api
{
  public class LifterLogger: Logger
  {
    public LifterLogger(DomainContext domain): base(domain) {}

    public void FromTask(
      LifterTask task,
      string operation,
      string message,
      Action<Log> useLevel = null,
      string data = ""
    ) {
      var log = Log.From(
        useLevel ?? Log.UseInfo(),
        Log.UseClass("wcs.lifter"),
        Log.UseOperation(operation),
        Log.UseIndex(task.Id.ToString()),
        Log.UseData(data),
        Log.UseMessage($"{task.LifterId} 号梯，{task.Floor} 楼，{message}")
      );

      Save(log);
    }

    public void FromLifter(
      string operation,
      string lifterId,
      string floor,
      string message,
      Action<Log> useLevel = null,
      string data = ""
    ) {
      var log = Log.From(
        useLevel ?? Log.UseInfo(),
        Log.UseClass("wcs.lifter"),
        Log.UseOperation(operation),
        Log.UseIndex(lifterId),
        Log.UseData(data),
        Log.UseMessage($"{lifterId} 号梯, {floor} 楼, {message}")
      );

      Save(log);
    }
  }
}
