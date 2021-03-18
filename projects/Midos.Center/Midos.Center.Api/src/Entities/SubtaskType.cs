using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SubtaskTypeParams = Midos.Center.Controllers.TaskTypeController.SubtaskTypeParams;

namespace Midos.Center.Entities
{
  [Table("subtask_types")]
  public class SubtaskType
  {
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("key")]
    public string Key { get; private set; }

    [Column("index")]
    public int Index { get; private set; }

    [Column("type_id")]
    public long TypeId { get; private set; }

    [ForeignKey("Subtype")]
    [Column("subtype_id")]
    public long SubtypeId { get; private set; }

    public TaskType Subtype { get; private set; }

    private SubtaskType() {}

    public SubtaskType Update(SubtaskTypeParams param)
    {
      Key = param.Key;
      Index = param.Index;
      SubtypeId = param.SubtypeId;

      return this;
    }

    public static SubtaskType From(
      string key,
      int index,
      long typeId,
      long subtypeId
    ) => new SubtaskType {
      Key = key,
      Index = index,
      TypeId = typeId,
      SubtypeId = subtypeId
    };

    public static SubtaskType From(SubtaskTypeParams param)
      => From(
        key: param.Key,
        index: param.Index,
        typeId: param.TypeId,
        subtypeId: param.SubtypeId
      );
  }
}
