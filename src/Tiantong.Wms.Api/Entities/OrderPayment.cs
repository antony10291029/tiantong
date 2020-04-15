using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("order_payments")]
  public class OrderPayment : Entity
  {
    public virtual double amount { get; set; }

    public virtual int order_id { get; set; }

    public virtual int index { get; set; }

    public virtual string comment { get; set; } = "";

    public virtual bool is_paid { get; set; } = false;

    public virtual DateTime due_time { get; set; } = DateTime.MinValue;

    public virtual DateTime paid_at { get; set; } = DateTime.MinValue;

  }
}
