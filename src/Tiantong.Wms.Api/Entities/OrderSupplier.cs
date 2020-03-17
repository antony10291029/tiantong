using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("order_suppliers")]
  public class OrderSupplier : Entity
  {
    [Key]
    public int id { get; set; }

    public int key { get; set; }

    public int order_id { get; set; }

    public int supplier_id { get; set; }

    public int[] item_keys { get; set; }
  }
}
