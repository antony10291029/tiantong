using Midos.Center.Entities;

namespace Midos.Center.Events
{
  public class TaskOrderCancelled
  {
    public const string Message = "tasks.cancelled";

    public string Key { get; init; }

    public long OrderId { get; init; }

    public string Data { get; init; }

    private TaskOrderCancelled() {}

    public static TaskOrderCancelled From(TaskType type, TaskOrder order)
      => new TaskOrderCancelled {
        Key = type.Key,
        OrderId = order.Id,
        Data = order.Data
      };
  }
}
