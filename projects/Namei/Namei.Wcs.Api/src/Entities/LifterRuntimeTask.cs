using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Wcs.Api
{
  [Table("lifter_runtime_tasks")]
  public class LifterRuntimeTask
  {
    [Column("lifter_id")]
    public string LifterId { get; private set; }

    [Column("barcode")]
    public string Barcode { get; private set; }

    [Column("lifter_task_id")]
    public int LifterTaskId { get; private set; }

    private LifterRuntimeTask()
    {

    }

    public static LifterRuntimeTask From(LifterTask task)
    {
      return new LifterRuntimeTask() {
        Barcode = task.Barcode,
        LifterId = task.LifterId,
        LifterTaskId = task.Id,
      };
    }

    public LifterRuntimeTask SetLifterTaskId(int lifterTaskId)
    {
      LifterTaskId = lifterTaskId;

      return this;
    }
  }
}
