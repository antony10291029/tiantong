using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Tiantong.Iot.Entities
{
  [Table("plc_state_errors")]
  public class PlcStateErrors
  {
    public virtual int id { get; set; }

    public virtual int plc_id { get; set; }

    public virtual int state_id { get; set; }

    public virtual string type { get; set; }

    public virtual string operation { get; set; }

    public virtual string detail { get; set; }

    public virtual DateTime created_at { get; set; } = DateTime.Now;
  }
}
