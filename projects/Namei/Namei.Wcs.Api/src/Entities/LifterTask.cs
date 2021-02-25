using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Wcs.Api
{
  [Table("lifter_tasks")]
  public class LifterTask
  {
    public int id { get; set; }

    public string lifter_id { get; set; }

    public string from { get; set; }

    public string to { get; set; }

    public string pallet_code { get; set; }

    public string task_id { get; set; }

    public string status { get; set; } = LifterTaskStatusType.Imported;

    public DateTime imported_at { get; set; } = DateTime.Now;

    public DateTime exported_at { get; set; } = DateTime.MinValue;

    public DateTime taken_at { get; set; } = DateTime.MinValue;
  }
}
