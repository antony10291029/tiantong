using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Tiantong.Iot.Protocol;

namespace Tiantong.Iot.Entities
{
  [Table("plc_states")]
  public class PlcState
  {
    [Key]
    public virtual int id { get; set; }

    public virtual int plc_id { get; set; }

    [MaxLength(20, ErrorMessage = "设备名称长度不可超过40")]
    public virtual string name { get; set; }

    [MaxLength(20, ErrorMessage = "编号长度不可超过20")]
    public virtual string number { get; set; }

    [PlcStateType]
    public virtual string type { get; set; }

    public virtual string address { get; set; }

    [Range(0, 200, ErrorMessage = "数据长度必须在 0 至 200 之间")]
    public virtual int length { get; set; }

    public virtual bool is_heartbeat { get; set; }

    public virtual int heartbeat_interval { get; set; } = 1000;

    [Range(100, 10000, ErrorMessage = "心跳范围必须在 100 至 10000 之间")]
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

    public static void EnsureAddress(string model, string address)
    {
      if (model == PlcModel.Test) {
        return;
      }

      IPlcReadRequest request = model switch {
        PlcModel.MC3EBinary => new MC3EBinaryReadRequest(),
        PlcModel.MC1EBinary => new MC1EBinaryReadRequest(),
        PlcModel.S7200Smart => new S7ReadRequest(),
        _ => throw KnownException.Error("PLC 型号不支持该数据类型"),
      };

      request.UseAddress(address);
    }
  }
}
