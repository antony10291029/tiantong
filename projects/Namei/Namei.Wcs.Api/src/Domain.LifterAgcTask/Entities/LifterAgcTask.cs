using Midos.Domain;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Wcs.Aggregates
{
  public class LifterAgcTask: IEntity
  {
    public long Id { get; private set; }

    [ForeignKey("Type")]
    public long TypeId { get; private set; }

    public long OrderId { get; private set; }

    public string PalletCode { get; private set; }

    public string Position { get; private set; }

    public string Destination { get; private set; }

    public string LifterId { get; private set; }

    public string Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime StartedAt { get; private set; }

    public DateTime ImportedAt { get; private set; }

    public DateTime ExportedAt { get; private set; }

    public DateTime ClosedAt { get; private set; }

    public LifterAgcTaskType Type { get; private set; }

    private LifterAgcTask() {}

    public static LifterAgcTask From(
      LifterAgcTaskType type,
      long orderId,
      string palletCode,
      string position,
      string destination,
      string lifterId
    ) => new LifterAgcTask {
      TypeId = type.Id,
      PalletCode = palletCode,
      Position = position,
      Destination = destination,
      OrderId = orderId,
      LifterId = lifterId,
      Status = LifterAgcTaskStatus.Created,
      CreatedAt = DateTime.Now,
      StartedAt = DateTime.MinValue,
      ImportedAt = DateTime.MinValue,
      ExportedAt = DateTime.MinValue,
      ClosedAt = DateTime.MinValue,
    };

    public void Start()
    {
      StartedAt = DateTime.Now;
      Status = LifterAgcTaskStatus.Started;
    }

    public void Export()
    {
      ExportedAt = DateTime.Now;
      Status = LifterAgcTaskStatus.Exported;
    }

    public void Finish()
    {
      ClosedAt = DateTime.Now;
      Status = LifterAgcTaskStatus.Finished;
    }

    public void Close()
    {
      ClosedAt = DateTime.Now;
      Status = LifterAgcTaskStatus.Closed;
    }
  }
}
