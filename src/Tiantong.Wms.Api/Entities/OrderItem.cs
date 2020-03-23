using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("order_items")]
  public class OrderItem : Entity
  {
    [Key]
    public int id { get; set; }

    public int order_id { get; set; }

    public int item_id { get; set; }

    public int supplier_id { get; set; }

    public double price { get; set; } = 0;

    public int quantity { get; set; }

    public int arrived_quantity { get; set; } = 0;
  }
}
