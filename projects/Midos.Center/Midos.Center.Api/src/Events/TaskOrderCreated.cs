using Midos.Center.Entities;

namespace Midos.Center.Events
{
  public class TaskOrderCreated
  {
    public const string Message = "task.order.created";

    public long OrderId { get; private set; }

    private TaskOrderCreated() {}

    public static string MessageFrom(TaskType type)
    {
      return $"{Message}.{type.Key}";
    }

    public static TaskOrderCreated From(TaskOrder order)
    {
      return new TaskOrderCreated {
        OrderId = order.Id
      };
    }
  }
}
