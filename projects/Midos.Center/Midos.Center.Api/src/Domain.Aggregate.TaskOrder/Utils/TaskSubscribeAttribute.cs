using DotNetCore.CAP;

namespace Midos.Center.Utils
{
  public class TaskCreatedAttribute: CapSubscribeAttribute
  {
    public TaskCreatedAttribute(string key)
      :base($"tasks.created.{key}") {}
  }

  public class TaskStartedAttribute: CapSubscribeAttribute
  {
    public TaskStartedAttribute(string key)
      :base($"tasks.started.{key}") {}
  }

  public class TaskFinishedAttribute: CapSubscribeAttribute
  {
    public TaskFinishedAttribute(string key)
      :base($"tasks.finished.{key}") {}
  }

  public class TaskCancelledAttribute: CapSubscribeAttribute
  {
    public TaskCancelledAttribute(string key)
      :base($"tasks.cancelled.{key}") {}
  }

  //

  public class SubtaskCreatedAttribute: CapSubscribeAttribute
  {
    public SubtaskCreatedAttribute(string key, string subkey)
      : base($"subtasks.created.{key}.{subkey}") {}
  }

  public class SubtaskStartedAttribute: CapSubscribeAttribute
  {
    public SubtaskStartedAttribute(string key, string subkey)
      : base($"subtasks.started.{key}.{subkey}") {}
  }

  public class SubtaskFinishedAttribute: CapSubscribeAttribute
  {
    public SubtaskFinishedAttribute(string key, string subkey)
      : base($"subtasks.finished.{key}.{subkey}") {}
  }

  public class SubtaskCancelledAttribute: CapSubscribeAttribute
  {
    public SubtaskCancelledAttribute(string key, string subkey)
      : base($"subtasks.cancelled.{key}.{subkey}") {}
  }
}
