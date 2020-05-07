using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Iot.Plc
{
  [Table("plc_states")]
  public class PlcState
  {
    public virtual int Id { get; set; }

    public virtual int PlcId { get; set; }

    public virtual string Name { get; set; }

    public virtual string Type { get; set; }

    public virtual string Length { get; set; }

    public virtual string Address { get; set; }

    public virtual bool IsHeartbeat { get; set; }

    public virtual int HeartbeatInterval { get; set; } = 1000;

    public virtual int HeartbeatMaxValue { get; set; } = 10000;

    public virtual bool IsCollect { get; set; }

    public virtual int CollectInterval { get; set; }

    public virtual bool OpenLog { get; set; }
  }
}
