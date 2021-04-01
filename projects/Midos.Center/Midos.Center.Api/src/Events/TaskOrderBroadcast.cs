using Midos.Domain;

namespace Midos.Center.Events
{
  public struct TaskOrderBroadcast
  {
    private const string _message = "tasks";

    public long OrderId { get; init; }

    public Record Data { get; init; }

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

    public static TaskOrderBroadcast From(long orderId, Record data)
      => new TaskOrderBroadcast {
        OrderId = orderId,
        Data = data
      };
  }
}
