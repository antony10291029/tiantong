using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Common.Api
{
  [Keyless]
  [Table("SapItemWarehouseInventory")]
  public class SapItemWarehouseInventory
  {
    public string U_I002 { get; set; }

    public string U_I003 { get; set; }

    public string ItemCode { get; set; }

    public string ItemName { get; set; }

    public string Unit { get; set; }

    public string WarehouseCode { get; set; }

    public string BatchCode { get; set; }

    public decimal Quantity { get; set; }

    public decimal? OpenQty { get; set; }
  }
}
