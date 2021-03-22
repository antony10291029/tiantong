using DotNetCore.CAP;
using Midos.Center.Events;

namespace Midos.Center.Utils
{
  class Utils
  {
    public static string TaskChanged(string status, string key)
      => $"tasks.{status}.{key}";

    public static string SubtaskChanged(string status, string key, string subkey)
      => $"tasks.{status}.{key}.{subkey}";
  }

  //

  public class TaskCreatedAttribute: CapSubscribeAttribute
  {
    public TaskCreatedAttribute(string key): base(
      Utils.TaskChanged(
        status: TaskOrderChanged.Created,
        key: key
      )
    ) {}
  }

  public class TaskStartedAttribute: CapSubscribeAttribute
  {
    public TaskStartedAttribute(string key): base(
      Utils.TaskChanged(
        status: TaskOrderChanged.Started,
        key: key
      )
    ) {}
  }

  public class TaskFinishedAttribute: CapSubscribeAttribute
  {
    public TaskFinishedAttribute(string key): base(
      Utils.TaskChanged(
        status: TaskOrderChanged.Finished,
        key: key
      )
    ) {}
  }

  public class TaskCancelledAttribute: CapSubscribeAttribute
  {
    public TaskCancelledAttribute(string key): base(
      Utils.TaskChanged(
        status: TaskOrderChanged.Finished,
        key: key)
    ) {}
  }

  //

  public class SubtaskCreatedAttribute: CapSubscribeAttribute
  {
    public SubtaskCreatedAttribute(string key, string subkey): base(
      Utils.SubtaskChanged(
        status: TaskOrderChanged.Created,
        key: key,
        subkey: subkey
      )
    ) {}
  }

  public class SubtaskStartedAttribute: CapSubscribeAttribute
  {
    public SubtaskStartedAttribute(string key, string subkey): base(
      Utils.SubtaskChanged(
        status: TaskOrderChanged.Started,
        key: key,
        subkey: subkey
      )
    ) {}
  }

  public class SubtaskFinishedAttribute: CapSubscribeAttribute
  {
    public SubtaskFinishedAttribute(string key, string subkey): base(
      Utils.SubtaskChanged(
        status: TaskOrderChanged.Finished,
        key: key,
        subkey: subkey
      )
    ) {}
  }

  public class SubtaskCancelledAttribute: CapSubscribeAttribute
  {
    public SubtaskCancelledAttribute(string key, string subkey): base(
      Utils.SubtaskChanged(
        status: TaskOrderChanged.Finished,
        key: key,
        subkey: subkey
      )
    ) {}
  }
}
