namespace Midos.Center.Events
{
  public class TaskOrderFinish
  {
    public const string Message = "tasks.finish";

    public string Key { get; init; }

    public long OrderId { get; init; }

    public string Data { get; init; }

    private TaskOrderFinish() {}

    public static TaskOrderFinish From(string key, long orderId, string data)
      => new TaskOrderFinish {
        Key = key,
        OrderId = orderId,
        Data = data
      };
  }
}
