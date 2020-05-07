using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Iot.Entities
{
  [Table("http_pusher_logs")]
  public class HttpPusherLog
  {
    [Key]
    public virtual int id { get; set; }

    public virtual int pusher_id { get; set; }

    public string request { get; set; }

    public string response { get; set; }

    public string status_code { get; set; }

    public DateTime created_at { get; set; } = DateTime.Now;
  }
}
