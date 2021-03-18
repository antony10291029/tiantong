using Midos.Center.Entities;

namespace Midos.Center.Events
{
  public class TaskOrderChange
  {
    public const string Update = "$tasks.update";

    public const string Start = "$tasks.start";

    public const string Finish = "$tasks.finish";

    public const string Cancel = "$tasks.cancel";

    public long OrderId { get; init; }

    public TaskData Data { get; init; }

    public static string Message(string message)
      => $"$tasks.{message}";

    private TaskOrderChange() {}

    public static TaskOrderChange From(long orderId, TaskData data)
      => new TaskOrderChange {
        OrderId = orderId,
        Data = data
      };
  }
}
