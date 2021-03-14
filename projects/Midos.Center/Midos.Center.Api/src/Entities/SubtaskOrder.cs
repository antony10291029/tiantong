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

    [Column("suborder_id")]
    public long SuborderId { get; private set; }
  }
}
