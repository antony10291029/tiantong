using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Iot.Entities
{
  [Table("http_pusher_errors")]
  public class HttpPusherError
  {
    [Key]
    public virtual int id { get; set; }

    public virtual int pusher_id { get; set; }

    public virtual string error { get; set; }

    public virtual string detail { get; set; }

    public DateTime created_at { get; set; } = DateTime.Now;
  }
}
