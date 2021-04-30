using System;

namespace Namei.Wcs.Api
{
  public interface ILifterLogger
  {
    void FromLifter(
      string operation,
      string lifterId,
      string floor,
      string message,
      Action<Log> useLevel = null,
      string data = ""
    );
  }

  public class LifterLogger: Logger, ILifterLogger
  {
    public LifterLogger(WcsContext domain): base(domain) {}

    public void FromTask(
      LifterTask task,
      string operation,
      string message,
      Action<Log> useLevel = null,
      string data = ""
    ) => Save(
      level: useLevel ?? Log.UseInfo(),
      klass: "wcs.lifter",
      operation: operation,
      index: task.Id.ToString(),
      data: data,
      message: $"{task.LifterId} 号梯，{task.Floor} 楼，{message}"
    );

    public void FromLifter(
      string operation,
      string lifterId,
      string floor,
      string message,
      Action<Log> useLevel = null,
      string data = ""
    ) => Save(
      level: useLevel ?? Log.UseInfo(),
      klass: "wcs.lifter",
      operation: operation,
      index: lifterId,
      data: data,
      message: $"{lifterId} 号梯, {floor} 楼, {message}"
    );
  }
}
