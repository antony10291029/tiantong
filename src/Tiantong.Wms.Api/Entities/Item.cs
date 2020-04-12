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

    [StringRange(4, 10, ErrorMessage = "规格编码长度必须在4～10之间")]
    public virtual string number { get; set; }

    [StringRange(1, 32, ErrorMessage = "规格名称长度必须在1～32之间")]
    public virtual string name { get; set; }

    [StringRange(1, 10, ErrorMessage = "规格单位长度必须在1～10之间")]
    public virtual string unit { get; set; }

    public virtual bool is_enabled { get; set; } = true;

    [ForeignKey("item_id")]
    public virtual List<Stock> stocks { get; set; }

  }
}
