using Midos.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SubtaskTypeParams = Midos.Center.Controllers.TaskTypeController.SubtaskTypeParams;

namespace Midos.Center.Aggregates
{
  public class SubtaskType: IEntity, IAggregateRoot
  {
    [Key]
    public long Id { get; private set; }

    public string Key { get; private set; }

    public int Index { get; private set; }

    public long TypeId { get; private set; }

    [ForeignKey("Subtype")]
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

    public static SubtaskType From(
      string key,
      int index,
      TaskType subtype
    ) => new SubtaskType {
      Key = key,
      Index = index,
      Subtype = subtype
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
