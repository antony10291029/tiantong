using Midos.Domain;

namespace Midos.Center.Events
{
  public struct SubtaskOrderCreate
  {
    public const string Message = "subtasks.create";

    public long OrderId { get; init; }

    public string Subkey { get; init; }

    public Record Data { get; init; }

    public string Code { get; init; }

    public static SubtaskOrderCreate From(
      long orderId,
      string subkey,
      Record data,
      string code = null
    ) {
      return new SubtaskOrderCreate {
        OrderId = orderId,
        Subkey = subkey,
        Data = data,
        Code = code,
      };
    }
  }
}
