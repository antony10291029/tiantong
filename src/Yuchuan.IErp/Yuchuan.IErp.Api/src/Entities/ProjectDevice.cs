using System.ComponentModel.DataAnnotations.Schema;

namespace Yuchuan.IErp.Api
{
  [Table("project_devices")]
  public class ProjectDevice
  {
    public int id { get; set; }

    public int project_id { get; set; }

    public int device_id { get; set; }

    public bool is_enabled { get; set; }
  }
}
