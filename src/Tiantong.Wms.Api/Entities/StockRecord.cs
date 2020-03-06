using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("stock_records")]
  public class StockRecord : Entity
  {
    [Key]
    public int id { get; set; }

    public int warehouse_id { get; set; }

    public int stock_id { get; set; }

    public int order_id { get; set; }

    public int order_item_id { get; set; }

    public int quantity { get; set; }

    public DateTime created_at { get; set; } = DateTime.Now;
  }
}
