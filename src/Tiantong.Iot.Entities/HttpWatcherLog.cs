using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Iot.Entities
{
  [Table("http_watcher_logs")]
  public class HttpWatcherLog
  {
    [Key]
    public int id { get; set; }

    public int plc_id { get; set; }

    public int state_id { get; set; }

    public int watcher_id { get; set; }

    public string request { get; set; }

    public string response { get; set; }

    public string status_code { get; set; }

    public DateTime created_at { get; set; } = DateTime.Now;
  }
}
