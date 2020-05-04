using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Iot.Entities
{
  [Table("plc_logs")]
  public class PlcLogs
  {
    [Key]
    public int id { get; set; }

    public int plc_id { get; set; }

    public string type { get; set; }

    public string operation { get; set; }

    public string detail { get; set; }

    public DateTime created_at { get; set; } = DateTime.Now;
  }
}
