using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wcs.Plc.Entities
{
  [Table("plc_state_logs")]
  public class PlcStateLog2
  {
    public virtual int id { get; set; }

    public virtual int plc_id { get; set; }

    public virtual int state_id { get; set; }

    public virtual bool read { get; set; }

    public virtual string value { get; set; }

    public virtual DateTime created_at { get; set; } = DateTime.Now;
  }
}
