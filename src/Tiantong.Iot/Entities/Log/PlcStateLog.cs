using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Iot.Entities
{
  [Table("plc_state_logs")]
  public class PlcStateLog
  {
    public virtual int id { get; set; }

    public virtual int plc_id { get; set; }

    public virtual int state_id { get; set; }

    public virtual string operation { get; set; }

    public virtual string value { get; set; }

    public virtual DateTime created_at { get; set; } = DateTime.Now;

  }
}
