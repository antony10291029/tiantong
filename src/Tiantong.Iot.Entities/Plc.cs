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
    public virtual int Id { get; set; } = 0;

    [Required]
    public virtual string Model { get; set; } = "test";

    [Required]
    public virtual string Name { get; set; }

    public virtual string Host { get; set; } = "";

    public virtual int Port { get; set; } = 0;

    public virtual bool IsRunning { get; set; } = false;

    [Required]
    public virtual DateTime CreatedAt { get; set; } = DateTime.Now;

    [ForeignKey("plc_id")]
    public List<PlcState> States { get; set; }

  }
}
