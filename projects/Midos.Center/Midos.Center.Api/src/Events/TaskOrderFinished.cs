using Midos.Center.Entities;

namespace Midos.Center.Events
{
  public class TaskOrderFinished
  {
    public const string Message = "tasks.finished";

    public string Key { get; init; }

    public long OrderId { get; init; }

    public string Data { get; init; }

    private TaskOrderFinished() {}

    public static TaskOrderFinished From(TaskType type, TaskOrder order)
      => new TaskOrderFinished {
        Key = type.Key,
        OrderId = order.Id
      };
  }
}
