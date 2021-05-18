using Midos.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Common.Entities
{
  [Table("wms_rest_inventory_quantity")]
  public class WmsInventoryRestQuantity
  {
    [Key]
    public string PalletCode { get; set; }

    public decimal RestQuantity { get; set; }
  }
}
