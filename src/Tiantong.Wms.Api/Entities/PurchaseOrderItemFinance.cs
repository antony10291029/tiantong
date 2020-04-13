using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("purchase_order_item_finances")]
  public class PurchaseOrderItemFinance : Entity
  {
    public virtual string name { get; set; }

    public virtual string specification { get; set; }

    public virtual string unit { get; set; }

    public virtual int quantity { get; set; }

    public virtual double price { get; set; }

    public virtual double amount { get; set; }

    public virtual double tax_rate { get; set; }

    public virtual double tax_amount { get; set; }

    public virtual string invoice_number { get; set; }

    public virtual string invoice_type { get; set; } = "";

  }
}
