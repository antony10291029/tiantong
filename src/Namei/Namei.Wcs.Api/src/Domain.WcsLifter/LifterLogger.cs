using System;
using System.Text.Json;

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

    void LogInfo(string operation, string lifterId, string floor, string message, object data = null)
      => FromLifter(operation, lifterId, floor, message, Log.UseInfo(), JsonSerializer.Serialize(data));

    void LogSuccess(string operation, string lifterId, string floor, string message, object data = null)
      => FromLifter(operation, lifterId, floor, message, Log.UseSuccess(), JsonSerializer.Serialize(data));

    void LogError(string operation, string lifterId, string floor, string message, object data = null)
      => FromLifter(operation, lifterId, floor, message, Log.UseDanger(), JsonSerializer.Serialize(data));
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
