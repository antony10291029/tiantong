using Midos.Center.Entities;

namespace Midos.Center.Events
{
  public struct TaskOrderChanged
  {
    public const string Created = "$tasks.created";

    public const string Started = "$tasks.started";

    public const string Finished = "$tasks.finished";

    public const string Cancelled = "$tasks.cancelled";

    public string Key { get; init; }

    public long OrderId { get; init; }

    public object Data { get; init; }

    public static TaskOrderChanged From(TaskType type, TaskOrder order)
      => new TaskOrderChanged {
        Key = type.Key,
        OrderId = order.Id,
        Data = order.Data
      };
  }
}
