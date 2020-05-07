using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Iot.Plc
{
  [Table("plcs")]
  public class Plc
  {
    public virtual int Id { get; set; } = 0;

    public virtual string Model { get; set; } = "test";

    public virtual string Name { get; set; }

    public virtual string Host { get; set; } = "";

    public virtual int Port { get; set; } = 0;

    public virtual bool IsRunning { get; set; } = false;

    public virtual DateTime CreatedAt { get; set; } = DateTime.Now;

    public List<PlcState> States { get; set; }

  }
}
