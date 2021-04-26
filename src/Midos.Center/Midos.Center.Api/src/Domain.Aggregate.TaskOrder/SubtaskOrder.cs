using Midos.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Midos.Center.Aggregates
{
  public class SubtaskOrder: IEntity, IAggregateRoot
  {
    [Key]
    public long Id { get; private set; }

    [ForeignKey("Subtype")]
    public long SubtypeId { get; private set; }

    [ForeignKey("Order")]
    public long OrderId { get; private set; }

    [ForeignKey("Suborder")]
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
