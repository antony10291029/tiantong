using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Iot.Entities
{
  [Table("plcs")]
  public class Plc
  {
    [Key]
    public virtual int id { get; set; }

    public virtual string model { get; set; }

    public virtual string name { get; set; }

    public virtual string host { get; set; }

    public virtual int port { get; set; }

    public virtual string comment { get; set; }

    public virtual DateTime created_at { get; set; } = DateTime.Now;

    [ForeignKey("plc_id")]
    public virtual List<PlcState> states { get; set; }

  }
}
