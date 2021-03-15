using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Midos.Center.Entities
{
  [Table("subtask_orders")]
  public class SubtaskOrder
  {
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("index")]
    public int Index { get; private set; }

    [Column("order_id")]
    public long OrderId { get; private set; }

    [ForeignKey("Suborder")]
    [Column("suborder_id")]
    public long SuborderId { get; private set; }

    public TaskOrder Suborder { get; private set; }

    private SubtaskOrder() {}

    public static SubtaskOrder From(SubtaskType type, TaskOrder order, TaskOrder suborder)
    {
      return new SubtaskOrder {
        Index = type.Index,
        OrderId = order.Id,
        Suborder = suborder
      };
    }
  }
}
