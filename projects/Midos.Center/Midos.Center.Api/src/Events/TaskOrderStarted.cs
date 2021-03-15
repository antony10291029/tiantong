using Midos.Center.Entities;

namespace Midos.Center.Events
{
  public class TaskOrderStarted
  {
    public const string Message = "tasks.started";

    public string Key { get; init; }

    public long OrderId { get; init; }

    public string Data { get; init; }

    private TaskOrderStarted() {}

    public static TaskOrderStarted From(TaskType type, TaskOrder order)
      => new TaskOrderStarted {
        Key = type.Key,
        OrderId = order.Id
      };
  }
}
