using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Common.Api
{
  [Keyless]
  [Table("wms_item_warehouse_inventory")]
  public class WmsItemWarehouseInventory
  {
    public string ItemCode { get; set; }

    public string WarehouseCode { get; set; }

    public string BatchCode { get; set; }

    public double Quantity { get; set; }
  }
}
