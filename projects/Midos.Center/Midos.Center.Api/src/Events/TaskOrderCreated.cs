using Midos.Center.Entities;

namespace Midos.Center.Events
{
  public class TaskOrderCreated
  {
    public const string Message = "tasks.created";

    public string Key { get; init; }

    public long OrderId { get; init; }

    private TaskOrderCreated() {}

    public static string MessageFrom(TaskType type)
    {
      return $"{Message}.{type.Key}";
    }

    public static TaskOrderCreated From(TaskType type, TaskOrder order)
    {
      return new TaskOrderCreated() {
        Key = type.Key,
        OrderId = order.Id
      };
    }
  }
}
