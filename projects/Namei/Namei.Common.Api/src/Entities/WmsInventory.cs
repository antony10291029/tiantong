using Midos.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Common.Entities
{
  [Table("wms_inventory")]
  public class WmsInventory: IEntity
  {
    [Key]
    [Column("ID")]
    public long Id { get; private set; }

    [Column("MOVE_TOOL_PALLET_NO")]
    public string MoveToolPalletNo { get; private set; }

    [Column("QTY_BASE_QTY")]
    public double? QtyBaseQty { get; private set; }

    [ForeignKey("Item")]
    [Column("SKU_ITEM_ID")]
    public long? SkuItemId { get; private set; }
  }
}
