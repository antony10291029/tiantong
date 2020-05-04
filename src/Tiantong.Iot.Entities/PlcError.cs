using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Iot.Entities
{
  [Table("plc_errors")]
  public class PlcError
  {
    [Key]
    public virtual int id { get; set; }

    public virtual int plc_id { get; set; }

    public virtual string error { get; set; }

    public virtual string detail { get; set; }

    public virtual DateTime created_at { get; set; } = DateTime.Now;


  }
}
