using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Midos.Center.Entities
{
  [Table("task_types")]
  public class TaskType
  {
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("key")]
    public string Key { get; private set; }

    [Column("name")]
    public string Name { get; private set; }

    [Column("data")]
    public string Data { get; private set; }

    [Column("comment")]
    public string Comment { get; private set; }

    public TaskType() {}
  }
}
