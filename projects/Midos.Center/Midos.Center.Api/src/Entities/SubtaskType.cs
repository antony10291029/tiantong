using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CreateParams = Midos.Center.Controllers.SubtaskTypeController.CreateParams;
using UpdateParams = Midos.Center.Controllers.SubtaskTypeController.UpdateParams;

namespace Midos.Center.Entities
{
  [Table("subtask_types")]
  public class SubtaskType
  {
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    public string Key { get; private set; }

    [Column("index")]
    public int Index { get; private set; }

    [Column("type_id")]
    public long TypeId { get; private set; }

    [ForeignKey("Subtype")]
    [Column("subtype_id")]
    public long SubtypeId { get; private set; }

    public TaskType Subtype { get; }

    private SubtaskType() {}

    public void UpdateFromRequest(UpdateParams param)
    {
      Index = param.Index;
      SubtypeId = param.SubtypeId;
    }

    public static SubtaskType FromRequest(CreateParams param)
    {
      return new SubtaskType {
        Key = param.Key,
        Index = param.Index,
        TypeId = param.SubtypeId,
        SubtypeId = param.SubtypeId,
      };
    }
  }
}
