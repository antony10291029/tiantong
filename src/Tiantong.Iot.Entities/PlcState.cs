using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Iot.Entities
{
  [Table("plc_states")]
  public class PlcState
  {
    [Key]
    public virtual int id { get; set; }

    public virtual int plc_id { get; set; }

    public virtual string name { get; set; }

    public virtual string type { get; set; }

    public virtual string address { get; set; }

    public virtual int length { get; set; }

    public virtual bool is_heartbeat { get; set; }

    public virtual int heartbeat_interval { get; set; } = 1000;

    public virtual int heartbeat_max_value { get; set; } = 10000;

    public virtual bool is_collect { get; set; }

    public virtual int collect_interval { get; set; } = 10000;

    public virtual bool is_read_log_on { get; set; }

    public virtual bool is_write_log_on { get; set; }

    [ForeignKey("state_id")]
    public virtual List<PlcStateHttpPusher> state_http_pushers { get; set; }

    [NotMapped]
    public List<HttpPusher> http_pushers
    {
      get => state_http_pushers?.Select(sp => sp.pusher).ToList();
    }
  }
}
