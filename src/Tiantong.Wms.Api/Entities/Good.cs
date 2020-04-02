using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("goods")]
  public class Good : Entity
  {
    public virtual int warehouse_id { get; set; }

    public virtual string number { get; set; }

    public virtual string name { get; set; }

    public virtual string comment { get; set; }

    public virtual bool is_enabled { get; set; } = true;

    public virtual bool is_deleted { get; set; } = false;

    public virtual DateTime created_at { get; set; } = DateTime.Now;

    [ForeignKey("good_id")]
    public virtual List<Item> items { get; set; }

  }
}
