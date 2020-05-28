using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Iot.Entities
{
  [Table("plc_state_http_pushers")]
  public class PlcStateHttpPusher
  {
    [Key]
    public virtual int id { get; set; }

    public virtual int state_id { get; set; }

    public virtual int pusher_id { get; set; }

    [ForeignKey("pusher_id")]
    public virtual HttpPusher pusher { get; set; }

  }

}
