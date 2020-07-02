using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Renet.Web.Attributes;
using Newtonsoft.Json;

namespace Yuchuan.IErp.Api
{
  [Table("devices")]
  public class Device
  {
    [Key]
    public int id { get; set; }

    [StringRange(2, 20, ErrorMessage = "名称长度必须在 2～20 之间")]

    public string name { get; set; }

    [StringLength(20, ErrorMessage = "编号长度必须小于 20")]
    public string number { get; set; } = "";

    [StringLength(200, ErrorMessage = "备注长度必须小于 200")]
    public string comment { get; set; } = "";

    public bool is_enabled { get; set; } = true;

    [JsonIgnore]
    [ForeignKey("device_id")]
    public List<DeviceState> state { get; set; }
  }
}
