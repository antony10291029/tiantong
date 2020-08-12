using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Wcs.Api
{
  [Table("rcs_tasks")]
  public class RcsTask
  {
    public int id { get; set; }

    public string type { get; set; }

    public string uuid { get; set; }

    public string device_type { get; set; }

    public string device_index { get; set; }

    public string action_task { get; set; }

    public string src { get; set; }

    public string dst { get; set; }

    public DateTime created_at { get; set; } = DateTime.Now;
  }
}
