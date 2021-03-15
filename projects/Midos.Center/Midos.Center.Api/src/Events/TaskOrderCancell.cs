namespace Midos.Center.Events
{
  public class TaskOrderCancell
  {
    public const string Message = "tasks.cancell";

    public long OrderId { get; init; }

    public string Data { get; init; }

    private TaskOrderCancell() {}

    public static TaskOrderCancell From(long orderId, string data)
      => new TaskOrderCancell {
        OrderId = orderId,
        Data = data
      };
  }
}
