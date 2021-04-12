using Midos.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Common.Entities
{
  [Table("wms_task")]
  public class WmsTask: IEntity
  {
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("CREATED_TIME")]
    public DateTime? CreatedAt { get; set; }

    [Column("MOVE_TOOL_PALLET_NO")]
    public string MoveToolPalletNo { get; set; }

    [Column("PICKED_QTY")]
    public double? PickedQty { get; set; }

    [ForeignKey("Item")]
    [Column("SKU_ITEM_ID")]
    public long SkuItemId { get; set; }

    [ForeignKey("ItemKey")]
    [Column("SKU_ITEM_KEY_ID")]
    public long SkuItemKeyId { get; set; }

    [ForeignKey("Location")]
    [Column("FROM_LOC_ID")]
    public long? FromLocationId { get; set; }

    [Column("MOVE_DOC_ID")]
    public long? MoveDocId { get; set; }

    public WmsItem Item { get; set; }

    public WmsItemKey ItemKey { get; set; }

    public WmsLocation Location { get; set; }
  }
}
