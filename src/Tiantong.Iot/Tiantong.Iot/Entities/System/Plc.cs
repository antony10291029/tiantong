using Midos.Web;
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

    [PlcModel]
    public virtual string model { get; set; }


    [MaxLength(20, ErrorMessage = "设备名称长度不可超过20")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "设备名称不可为空")]
    public virtual string name { get; set; }

    [MaxLength(20, ErrorMessage = "编号长度不可超过20")]
    public virtual string number { get; set; }

    [IPAddress]
    public virtual string host { get; set; }

    [IPPort]
    public virtual int port { get; set; }

    [MaxLength(255, ErrorMessage = "备注长度不可超过 255")]
    public virtual string comment { get; set; }

    public virtual DateTime created_at { get; set; } = DateTime.Now;

    [ForeignKey("plc_id")]
    public virtual List<PlcState> states { get; set; }
  }
}
