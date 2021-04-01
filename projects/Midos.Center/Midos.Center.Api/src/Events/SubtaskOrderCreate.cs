using Midos.Center.Entities;

namespace Midos.Center.Events
{
  public struct SubtaskOrderCreate
  {
    public const string Message = "subtasks.create";

    public long OrderId { get; init; }

    public string Subkey { get; init; }

    public TaskData Data { get; init; }

    public static SubtaskOrderCreate From(
      long orderId,
      string subkey,
      TaskData data
    ) {
      return new SubtaskOrderCreate {
        OrderId = orderId,
        Subkey = subkey,
        Data = data,
      };
    }
  }
}
