using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Iot.Entities
{
  [Table("plc_states")]
  public class PlcState
  {
    public virtual int id { get; set; }

    public virtual int plc_id { get; set; }

    public virtual string name { get; set; }

    public virtual string type { get; set; }

    public virtual string length { get; set; }

    public virtual string address { get; set; }

    public virtual bool is_heartbeat { get; set; }

    public virtual int heartbeat_interval { get; set; } = 1000;

    public virtual bool is_collect { get; set; }

    public virtual int collect_interval { get; set; }

    public virtual bool open_log { get; set; }

  }
}
