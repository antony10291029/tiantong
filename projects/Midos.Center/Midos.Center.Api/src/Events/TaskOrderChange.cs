using Midos.Domain;

namespace Midos.Center.Events
{
  public struct TaskOrderChange
  {
    public const string Update = "$tasks.update";

    public const string Start = "$tasks.start";

    public const string Finish = "$tasks.finish";

    public const string Cancel = "$tasks.cancel";

    public long OrderId { get; init; }

    public Record Data { get; init; }

    public static string Message(string message)
      => $"$tasks.{message}";

    public static TaskOrderChange From(long orderId, Record data)
      => new TaskOrderChange {
        OrderId = orderId,
        Data = data
      };
  }
}
