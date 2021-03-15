using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Midos.Center.Events;

namespace Midos.Center.Entities
{

  [Table("task_orders")]
  public class TaskOrder
  {
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("type_id")]
    public long TypeId { get; set; }

    [Column("status")]
    public string Status { get; set; }

    [Column("data")]
    public string Data { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("started_at")]
    public DateTime StartedAt { get; set; }

    [Column("closed_at")]
    public DateTime ClosedAt { get; set; }

    public bool IsCreated()
      => Status == TaskOrderStatus.Created;

    public bool IsStarted()
      => Status != TaskOrderStatus.Created;

    public bool IsFinished()
      => Status != TaskOrderStatus.Finished;

    public bool IsCancelled()
      => Status != TaskOrderStatus.Cancelled;

    public bool IsClosed()
      => IsFinished() || IsCancelled();

    public void Start(string data)
    {
      Data = data;
      StartedAt = DateTime.Now;
      Status = TaskOrderStatus.Started;
    }

    public void Cancell(string data)
    {
      Data = data;
      ClosedAt = DateTime.Now;
      Status = TaskOrderStatus.Cancelled;
    }

    public void Finish(string data)
    {
      Data = data;
      ClosedAt = DateTime.Now;
      Status = TaskOrderStatus.Finished;
    }

    public static TaskOrder From(TaskType type, TaskOrderCreate param)
    {
      return new TaskOrder {
        TypeId = type.Id,
        Status = TaskOrderStatus.Created,
        Data = param.Data,
        CreatedAt = DateTime.Now,
        StartedAt = DateTime.MinValue,
        ClosedAt = DateTime.MinValue
      };
    }

    public static TaskOrder From(TaskType type, SubtaskOrderCreate param)
    {
      return new TaskOrder {
        TypeId = type.Id,
        Status = TaskOrderStatus.Created,
        Data = param.Data,
        CreatedAt = DateTime.Now,
        StartedAt = DateTime.MinValue,
        ClosedAt = DateTime.MinValue
      };
    }
  }
}
