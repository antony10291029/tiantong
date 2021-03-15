using Midos.Center.Entities;

namespace Midos.Center.Events
{
  public class SubtaskOrderCreated
  {
    public const string Message = "subtasks.created.private";

    public string Key { get; init; }

    public string Subkey { get; init; }

    public string SubtypeKey { get; init; }

    public long OrderId { get; init; }

    public long SuborderId { get; init; }

    public string Data { get; init; }

    private SubtaskOrderCreated() {}

    public static SubtaskOrderCreated From(
      TaskType type,
      SubtaskType subtype,
      TaskOrder order,
      TaskOrder suborder
    ) {
      return new SubtaskOrderCreated {
        Key = type.Key,
        Subkey = subtype.Key,
        SubtypeKey = subtype.Subtype.Key,
        OrderId = order.Id,
        SuborderId = suborder.Id,
        Data = suborder.Data
      };
    }
  }
}
