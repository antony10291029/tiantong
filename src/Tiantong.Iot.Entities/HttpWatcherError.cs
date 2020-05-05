using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Iot.Entities
{
  [Table("http_watcher_errors")]
  public class HttpWatcherError
  {
    [Key]
    public int id { get; set; }

    public int plc_id { get; set; }

    public int state_id { get; set; }

    public int watcher_id { get; set; }

    public virtual string error { get; set; }

    public virtual string detail { get; set; }

    public virtual DateTime created_at { get; set; } = DateTime.Now;

  }
}
