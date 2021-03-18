using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using TaskTypeParams = Midos.Center.Controllers.TaskTypeController.TaskTypeParams;

namespace Midos.Center.Entities
{
  public class TaskData: Dictionary<string, string> {}

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

    [JsonIgnore]
    [Column("data")]
    public string _data { get; private set; }

    [Column("comment")]
    public string Comment { get; private set; }

    [ForeignKey("TypeId")]
    public List<SubtaskType> Subtypes { get; private set; }

    [NotMapped]
    public TaskData Data
    {
      get => FromData(_data);
      private set => _data = ToData(value);
    }

    private TaskType() {}

    public List<SubtaskType> Update(TaskTypeParams param)
    {
      Key = param.Key;
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

    //

    public static TaskType From(
      string key,
      string name,
      string comment,
      TaskData data,
      List<SubtaskType> subtypes = null
    ) => new TaskType {
      Key = key,
      Name = name,
      Comment = comment,
      Subtypes = subtypes,
      Data = data,
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

    //

    public static string ToData(TaskData data)
      => data == null ? "{}": JsonSerializer.Serialize(data);

    public static TaskData FromData(string data)
      => JsonSerializer.Deserialize<TaskData>(data ?? "{}");

    public static string MergeData(string data, TaskData newData)
    {
      var tmp = JsonSerializer.Deserialize<Dictionary<string, string>>(data);

      newData.ToList().ForEach(kv => tmp[kv.Key] = kv.Value);

      return JsonSerializer.Serialize(tmp);
    }
  }
}
