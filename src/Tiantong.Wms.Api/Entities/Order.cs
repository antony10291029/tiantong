using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Tiantong.Wms.Api
{
  [Table("orders")]
  public class Order : Entity
  {
    public virtual int warehouse_id { get; set; }

    public virtual string number { get; set; }

    public virtual int operator_id { get; set; }

    public virtual int applicant_id { get; set; }

    public virtual int department_id { get; set; }

    public virtual int supplier_id { get; set; }

    [JsonIgnore]
    public virtual string type { get; set; }

    public virtual string status { get; set; } = "";

    public virtual string comment { get; set; } = "";

    public virtual DateTime due_time { get; set; } = DateTime.MinValue;

    public virtual DateTime created_at { get; set; } = DateTime.Now;

    public virtual DateTime finished_at { get; set; } = DateTime.MinValue;

    [ForeignKey("order_id")]
    public virtual List<OrderPayment> payments { get; set; }

    [ForeignKey("order_id")]
    public virtual List<OrderItem> items { get; set; }
  }
}
