namespace Midos.Center.Events
{
  public class TaskOrderBroadcast
  {
    private const string _message = "tasks";

    public long OrderId { get; init; }

    public object Data { get; init; }

    private TaskOrderBroadcast() {}

    public static string Message(string status, string key)
      => $"{_message}.{status}.{key}";

    public static string Created(string key)
      => Message("created", key);

    public static string Started(string key)
      => Message("started", key);

    public static string Finished(string key)
      => Message("finished", key);

    public static string Cancelled(string key)
      => Message("cancelled", key);

    public static TaskOrderBroadcast From(long orderId, object data)
      => new TaskOrderBroadcast {
        OrderId = orderId,
        Data = data
      };
  }
}
