using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("orders")]
  public class Order : Entity
  {
    [Key]
    public int id { get; set; }

    public int warehouse_id { get; set; }

    public string number { get; set; }

    public string type { get; set; }

    public int category_id { get; set; }

    public int operator_id { get; set; }

    public string status { get; set; } = "";

    public string comment { get; set; } = "";

    public DateTime due_time { get; set; } = DateTime.Now;

    public DateTime created_at { get; set; } = DateTime.Now;

    public DateTime finished_at { get; set; } = DateTime.MinValue;
  }
}
