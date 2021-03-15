using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    public static TaskOrder From(TaskType type, string data)
    {
      return new TaskOrder {
        TypeId = type.Id,
        Status = TaskOrderStatus.Created,
        Data = data,
        CreatedAt = DateTime.Now,
        StartedAt = DateTime.MinValue,
        ClosedAt = DateTime.MinValue
      };
    }
  }
}
