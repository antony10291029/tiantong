using Midos.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using TaskTypeParams = Midos.Center.Controllers.TaskTypeController.TaskTypeParams;

namespace Midos.Center.Aggregates
{
  public class TaskType: IEntity
  {
    [Key]
    public long Id { get; private set; }

    public string Key { get; private set; }

    public string Name { get; private set; }

    public bool HasCode { get; private set; }

    [JsonIgnore]
    [Column("Data")]
    public string _data { get; private set; }

    public string Comment { get; private set; }

    [ForeignKey("TypeId")]
    public List<SubtaskType> Subtypes { get; private set; }

    [NotMapped]
    public Record Data
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
      HasCode = param.HasCode;

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
      bool hasCode,
      string comment,
      Record data,
      List<SubtaskType> subtypes = null
    ) => new TaskType {
      Key = key,
      Name = name,
      HasCode = hasCode,
      Comment = comment,
      Subtypes = subtypes,
      Data = data,
    };

    public static TaskType From(TaskTypeParams param)
      => From(
        key: param.Key,
        name: param.Name,
        data: param.Data,
        hasCode: param.HasCode,
        comment: param.Comment,
        subtypes: param.Subtypes.Select(type => SubtaskType.From(
          key: type.Key,
          index: type.Index,
          typeId: 0,
          subtypeId: type.SubtypeId
        )).ToList()
      );

    //

    public static string ToData(Record data)
      => data == null ? "{}": JsonSerializer.Serialize(data);

    public static Record FromData(string data)
      => JsonSerializer.Deserialize<Record>(data ?? "{}");

    public static string MergeData(string data, Record newData)
    {
      var tmp = JsonSerializer.Deserialize<Dictionary<string, string>>(data);

      newData.ToList().ForEach(kv => tmp[kv.Key] = kv.Value);

      return JsonSerializer.Serialize(tmp);
    }
  }
}
