using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Midos.Center.Entities
{
  [Table("task_orders")]
  public class TaskOrder
  {
    [Key]
    [Column("Id")]
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
  }
}
