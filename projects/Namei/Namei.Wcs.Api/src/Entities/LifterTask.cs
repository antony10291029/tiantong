using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Midos.Domain;

namespace Namei.Wcs.Api
{
  [Table("lifter_tasks")]
  public class LifterTask: IEntity
  {
    [Key]
    [Column("id")]
    public long Id { get; private set; }

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

    public void Close()
    {
      Status = LifterTaskStatus.Closed;
    }

    public static LifterTask FromImportedEvent(LifterTaskImported param)
    {
      return new LifterTask {
        LifterId = param.LifterId,
        Floor = param.Floor,
        Destination = param.Destination,
        Barcode = param.Barcode,
        TaskCode = param.TaskCode,
        Status = LifterTaskStatus.Imported,
        ImportedAt = DateTime.Now,
        ExportedAt = DateTime.MinValue,
        TakenAt = DateTime.MinValue
      };
    }
  }
}
