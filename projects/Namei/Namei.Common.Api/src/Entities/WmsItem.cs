using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Common.Entities
{
  [Table("wms_item")]
  public class WmsItem
  {
    [Key]
    [Column("ID")]
    public long Id { get; private set; }

    public string Code { get; private set; }
  }
}
