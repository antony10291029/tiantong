using Midos.Center.Events;
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

    [ForeignKey("Subtype")]
    [Column("subtype_id")]
    public long SubtypeId { get; private set; }

    [ForeignKey("Order")]
    [Column("order_id")]
    public long OrderId { get; private set; }

    [ForeignKey("Suborder")]
    [Column("suborder_id")]
    public long SuborderId { get; private set; }

    public TaskOrder Order { get; private set; }

    public TaskOrder Suborder { get; private set; }

    public SubtaskType Subtype { get; private set; }

    private SubtaskOrder() {}

    public static SubtaskOrder From(
      SubtaskType subtype,
      TaskOrder order,
      TaskOrder suborder
    ) => new SubtaskOrder {
        Order = order,
        Suborder = suborder,
        Subtype = subtype
      };
  }
}
