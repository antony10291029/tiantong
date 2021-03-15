using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CreateParams = Midos.Center.Controllers.TaskTypeController.CreateParams;
using UpdateParams = Midos.Center.Controllers.TaskTypeController.UpdateParams;

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

    private TaskType() {}

    public void UpdateFromRequest(UpdateParams param)
    {
      Name = param.Name;
      Data = param.Data;
      Comment = param.Comment;
    }

    public static TaskType FromRequest(CreateParams param)
    {
      return new TaskType {
        Key = param.Key,
        Name = param.Name,
        Data = param.Data,
        Comment = param.Comment
      };
    }
  }
}
