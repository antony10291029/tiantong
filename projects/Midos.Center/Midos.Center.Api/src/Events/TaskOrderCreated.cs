using Midos.Center.Entities;

namespace Midos.Center.Events
{
  public class TaskOrderCreated
  {
    public const string Message = "tasks.created";

    public string Key { get; init; }

    public long OrderId { get; init; }

    public string Data { get; init; }

    private TaskOrderCreated() {}

    public static TaskOrderCreated From(string key, long orderId, string data)
      => new TaskOrderCreated {
        Key = key,
        OrderId = orderId,
        Data = data
      };

    public static TaskOrderCreated From(TaskType type, TaskOrder order)
      => From(type.Key, order.Id, order.Data);

    public static TaskOrderCreated From(SubtaskOrderCreated param)
      => From(param.SubtypeKey, param.SuborderId, param.Data);
  }
}
