using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Namei.Common.Entities
{
  [Keyless]
  [Table("Report_Warehouse_BatchOnhand")]
  public class SapItemBatch
  {
    [Column("DistNumber")]
    public string BatchId { get; private set; }

    [Column("WhsType")]
    public string WarehouseType { get; private set; }

    [Column("WhsCode")]
    public string WarehouseCode { get; private set; }

    [Column("ItmsGrpNam")]
    public string ItemGroupName { get; private set; }

    [Column("ItemMidclass")]
    public string ItemMidclass { get; private set; }

    [Column("ItemSubclass")]
    public string ItemSubclass { get; private set; }

    [Column("ItemCode")]
    public string ItemCode { get; private set; }

    [Column("ItemName")]
    public string ItemName { get; private set; }

    [Column("Quantity")]
    public decimal Quantity { get; private set; }
  }
}
