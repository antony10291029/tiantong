using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Wcs.Api
{
  [Table("lifter_tasks")]
  public class LifterTask
  {
    [Key]
    [Column("id")]
    public int Id { get; private set; }

    [Column("lifter_id")]
    public string LifterId { get; private set; }

    [Column("floor")]
    public string Floor { get; private set; }

    [Column("destination")]
    public string Destination { get; private set; }

    [Column("barcode")]
    public string Barcode { get; private set; }

    [Column("task_code")]
    public string TaskCode { get; private set; }

    [Column("status")]
    public string Status { get; private set; }

    [Column("imported_at")]
    public DateTime ImportedAt { get; private set; }

    [Column("exported_at")]
    public DateTime ExportedAt { get; private set; }

    [Column("taken_at")]
    public DateTime TakenAt { get; private set; }

    private LifterTask() {}

    public static LifterTask From(
      string lifterId,
      string floor,
      string destination = "",
      string barcode = "",
      string taskCode = ""
    ) {
      return new LifterTask() {
        LifterId = lifterId,
        Floor = floor,
        Destination = destination,
        Barcode = barcode,
        TaskCode = taskCode,
        Status = LifterTaskStatus.Imported,
        ImportedAt = DateTime.MinValue,
        ExportedAt = DateTime.MinValue,
        TakenAt = DateTime.MinValue,
      };
    }

    public bool IsExported()
    {
      return ExportedAt != DateTime.MinValue;
    }

    public LifterTask SetImported()
    {
      Status = LifterTaskStatus.Imported;
      ImportedAt = DateTime.Now;

      return this;
    }

    public LifterTask SetExported()
    {
      Status = LifterTaskStatus.Exported;
      ExportedAt = DateTime.Now;

      return this;
    }

    public LifterTask SetTaken()
    {
      Status = LifterTaskStatus.Taken;
      TakenAt = DateTime.Now;

      return this;
    }
  }
}
