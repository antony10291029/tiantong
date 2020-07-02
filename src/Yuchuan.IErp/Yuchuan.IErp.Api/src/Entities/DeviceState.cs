using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yuchuan.IErp.Api
{
  [Table("device_states")]
  public class DeviceState
  {
    [Key]
    public int id { get; set; }

    public int device_id { get; set; }

    public string state { get; set; }

    public string mode { get; set; }

    public string position { get; set; }

    public string message { get; set; }
    
    public DateTime created_at { get; set; } = DateTime.Now;
  }
}