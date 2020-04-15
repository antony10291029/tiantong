using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("invoices")]
  public class Invoice : Entity
  {
    public virtual string name { get; set; }

    public virtual string specification { get; set; }

    public virtual string unit { get; set; }

    public virtual int quantity { get; set; }

    public virtual double price { get; set; }

    public virtual double amount { get; set; }

    public virtual double tax_rate { get; set; }

    public virtual double tax_amount { get; set; }

    public virtual string type { get; set; } = "";

    public virtual string number { get; set; }

  }
}
