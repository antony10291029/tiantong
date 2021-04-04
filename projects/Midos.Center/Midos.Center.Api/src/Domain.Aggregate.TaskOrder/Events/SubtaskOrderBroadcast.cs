using Midos.Center.Aggregates;

namespace Midos.Center.Events
{
  public struct SubtaskOrderBroadcast
  {
    private const string _message = "tasks";

    public long OrderId { get; init; }

    public long SuborderId { get; init; }

    public object Data { get; init; }

    public object Subdata { get; init; }

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

    public static SubtaskOrderBroadcast From(
      TaskOrder order,
      long suborderId,
      object subdata
    ) => new SubtaskOrderBroadcast {
        OrderId = order.Id,
        Data = order.Data,
        SuborderId = suborderId,
        Subdata = subdata
      };
  }
}
