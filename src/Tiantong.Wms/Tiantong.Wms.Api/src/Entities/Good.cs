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

    [StringRange(4, 10, ErrorMessage = "货品编码长度必须在4～10之间")]
    public virtual string number { get; set; }

    [StringRange(1, 32, ErrorMessage = "货品名称长度必须在1～32之间")]
    public virtual string name { get; set; }

    [MaxLength(255, ErrorMessage = "货品备注长度最大为255")]
    public virtual string comment { get; set; } = "";

    public virtual bool is_enabled { get; set; } = true;

    public virtual DateTime created_at { get; set; } = DateTime.Now;

    [ForeignKey("good_id")]
    public virtual List<Item> items { get; set; }

  }
}
