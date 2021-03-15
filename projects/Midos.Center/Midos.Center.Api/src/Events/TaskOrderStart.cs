namespace Midos.Center.Events
{
  public class TaskOrderStart
  {
    public const string Message = "tasks.start";

    public long OrderId { get; init; }

    public string Data { get; init; }

    private TaskOrderStart() {}

    public static TaskOrderStart From(long orderId, string data)
      => new TaskOrderStart {
        OrderId = orderId,
        Data = data
      };
  }
}
