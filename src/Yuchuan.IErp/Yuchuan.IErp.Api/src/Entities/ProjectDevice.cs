using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yuchuan.IErp.Api
{
  [Table("project_devices")]
  public class ProjectDevice
  {
    [Key]
    public int id { get; set; }

    public int project_id { get; set; }

    public int device_id { get; set; }

    [JsonIgnore]
    [ForeignKey("device_id")]
    public Device device { get; set; }
  }
}
