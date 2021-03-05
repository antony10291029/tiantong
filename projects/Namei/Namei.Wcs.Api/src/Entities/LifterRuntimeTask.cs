using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Wcs.Api
{
  [Table("lifter_runtime_tasks")]
  public class LifterRuntimeTask
  {
    [Key]
    [Column("barcode")]
    public string Barcode { get; private set; }

    [Column("lifter_task_id")]
    public int LifterTaskId { get; private set; }

    [ForeignKey("LifterTaskId")]
    public LifterTask LifterTask { get; private set; }

    private LifterRuntimeTask() {}

    public static LifterRuntimeTask From(LifterTask task)
    {
      return new LifterRuntimeTask() {
        Barcode = task.Barcode,
        LifterTaskId = task.Id,
      };
    }

    public LifterRuntimeTask UseLifterTaskId(int lifterTaskId)
    {
      LifterTaskId = lifterTaskId;

      return this;
    }
  }
}
