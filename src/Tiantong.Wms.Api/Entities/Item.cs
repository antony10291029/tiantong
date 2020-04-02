using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("items")]
  public class Item : Entity
  {
    public virtual int warehouse_id { get; set; }

    public virtual int good_id { get; set; }

    public virtual int index { get; set; }

    public virtual string number { get; set; }

    public virtual string name { get; set; }

    public virtual string unit { get; set; }

    public virtual bool is_enabled { get; set; } = true;

    public virtual bool is_deleted { get; set; } = false;

    [ForeignKey("item_id")]
    public virtual List<Stock> stocks { get; set; }

  }
}
