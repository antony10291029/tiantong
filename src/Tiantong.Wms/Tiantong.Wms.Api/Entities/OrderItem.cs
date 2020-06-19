using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("order_items")]
  public class OrderItem : Entity
  {
    public virtual int order_id { get; set; }

    public virtual int good_id { get; set; }

    public virtual int item_id { get; set; }

    public virtual int invoice_id { get; set; }

    public virtual int index { get; set; }

    public virtual double price { get; set; }

    public virtual int quantity { get; set; }

    public virtual string comment { get; set; }

    public virtual string delivery_cycle { get; set; }

    public virtual DateTime arrived_at { get; set; } = DateTime.Now;

    [ForeignKey("invoice_id")]
    public virtual Invoice invoice { get; set; }

    [ForeignKey("order_item_id")]
    public virtual List<OrderItemProject> projects { get; set; }
  }
}
