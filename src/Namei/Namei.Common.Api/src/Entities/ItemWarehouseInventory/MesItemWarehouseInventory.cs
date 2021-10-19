using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Common.Api
{
  [Keyless]
  [Table("MesItemWarehouseInventory")]
  public class MesItemWarehouseInventory
  {
    public string? ItemCode { get; set; }

    public string? WarehouseCode { get; set; }

    public string? BatchCode { get; set; }

    public double? Quantity { get; set; }

    public double? ReportingQuantity { get; set; }

    public double? ConfirmingQty { get; set; }
  }
}
