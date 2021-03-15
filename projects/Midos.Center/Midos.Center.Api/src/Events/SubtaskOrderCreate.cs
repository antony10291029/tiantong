namespace Midos.Center.Events
{
  public class SubtaskOrderCreate
  {
    public const string Message = "subtasks.create";

    public long OrderId { get; private set; }

    public string Subkey { get; private set; }

    public string Data { get; private set; }

    private SubtaskOrderCreate() {}

    public static string FromMessage(string key, string subkey)
    {
      return $"{Message}.{key}.{subkey}";
    }

    public SubtaskOrderCreate From(
      long orderId,
      string subkey,
      string data
    ) {
      return new SubtaskOrderCreate {
        OrderId = orderId,
        Subkey = subkey,
        Data = data,
      };
    }
  }
}
