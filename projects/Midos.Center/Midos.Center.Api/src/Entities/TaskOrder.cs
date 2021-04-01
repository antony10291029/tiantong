using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Midos.Center.Events;
using System.Text.Json.Serialization;

namespace Midos.Center.Entities
{
  public class TaskOrder
  {
    [Key]
    public long Id { get; private set; }

    public long TypeId { get; private set; }

    public string Status { get; private set; }

    [JsonIgnore]
    [Column("Data")]
    public string _data { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime StartedAt { get; private set; }

    public DateTime ClosedAt { get; private set; }

    public TaskType Type { get; private set; }

    [NotMapped]
    public TaskData Data
    {
      get => TaskType.FromData(_data);
      set => _data = TaskType.ToData(value);
    }

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

    public void UseData(TaskData data)
      => _data = TaskType.MergeData(_data, data);

    public void Start(TaskData data)
    {
      UseData(data);
      StartedAt = DateTime.Now;
      Status = TaskOrderStatus.Started;
    }

    public void Cancel(TaskData data)
    {
      UseData(data);
      ClosedAt = DateTime.Now;
      Status = TaskOrderStatus.Cancelled;
    }

    public void Finish(TaskData data)
    {
      UseData(data);
      ClosedAt = DateTime.Now;
      Status = TaskOrderStatus.Finished;
    }

    public static TaskOrder From(TaskType type, TaskOrderCreate param)
    {
      return new TaskOrder {
        TypeId = type.Id,
        Status = TaskOrderStatus.Created,
        _data = TaskType.MergeData(type._data, param.Data),
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
        _data = TaskType.MergeData(type._data, param.Data),
        CreatedAt = DateTime.Now,
        StartedAt = DateTime.MinValue,
        ClosedAt = DateTime.MinValue
      };
    }
  }
}
