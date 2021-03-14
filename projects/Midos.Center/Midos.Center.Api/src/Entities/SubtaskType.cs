using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Midos.Center.Entities
{
  [Table("subtask_types")]
  public class SubtaskType
  {
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("index")]
    public int Index { get; private set; }

    [Column("type_id")]
    public long TypeId { get; private set; }

    [Column("subtype_id")]
    public long SubtypeId { get; private set; }

    private SubtaskType() {}

    public static SubtaskType From(int index, long typeId, long subtypeId)
    {
      return new SubtaskType {
        Index = index,
        TypeId = typeId,
        SubtypeId = subtypeId,
      };
    }
  }
}
