using Midos.Center.Entities;

namespace Midos.Center.Events
{
  public class SubtaskOrderBroadcast
  {
    private const string _message = "tasks";

    public long OrderId { get; init; }

    public long SuborderId { get; init; }

    public string Data { get; init; }

    private SubtaskOrderBroadcast() {}

    public static string Message(string status, string key, string subkey)
      => $"{_message}.{status}.{key}.{subkey}";

    public static string Created(string key, string subkey)
      => Message("created", key, subkey);

    public static string Started(string key, string subkey)
      => Message("started", key, subkey);

    public static string Finished(string key, string subkey)
      => Message("finished", key, subkey);

    public static string Cancelled(string key, string subkey)
      => Message("cancelled", key, subkey);

    public static SubtaskOrderBroadcast From(long orderId, long suborderId, string data)
      => new SubtaskOrderBroadcast {
        OrderId = orderId,
        SuborderId = suborderId,
        Data = data
      };

    public static SubtaskOrderBroadcast From(SubtaskOrder suborder, string data)
      => From(suborder.OrderId, suborder.SuborderId, data);
  }
}
