using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TaskTypeParams = Midos.Center.Controllers.TaskTypeController.TaskTypeParams;

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

    [ForeignKey("TypeId")]
    public List<SubtaskType> Subtypes { get; private set; }

    private TaskType() {}

    public List<SubtaskType> Update(TaskTypeParams param)
    {
      Name = param.Name;
      Data = param.Data;
      Comment = param.Comment;

      var ids = param.Subtypes.Select(subtype => subtype.Id);
      var removedSubtypes = Subtypes
        .Where(subtype => !ids.Any(id => id == subtype.Id))
        .ToList();

      Subtypes = Subtypes
        .Where(subtype => ids.Any(id => id == subtype.Id))
        .Select(subtype => subtype.Update(
            param.Subtypes.First(param => param.Id == subtype.Id)
          )
        ).ToList();

      Subtypes.AddRange(
        param.Subtypes
          .Where(subtype => subtype.Id == 0)
          .Select(subtype => SubtaskType.From(subtype))
      );

      return removedSubtypes;
    }

    public static TaskType From(
      string key,
      string name,
      string data,
      string comment,
      List<SubtaskType> subtypes = null
    ) => new TaskType {
      Key = key,
      Name = name,
      Data = data,
      Comment = comment,
      Subtypes = subtypes,
    };

    public static TaskType From(TaskTypeParams param)
      => From(
        key: param.Key,
        name: param.Name,
        data: param.Data,
        comment: param.Comment,
        subtypes: param.Subtypes.Select(type => SubtaskType.From(
          key: type.Key,
          index: type.Index,
          typeId: 0,
          subtypeId: type.SubtypeId
        )).ToList()
      );
  }
}
