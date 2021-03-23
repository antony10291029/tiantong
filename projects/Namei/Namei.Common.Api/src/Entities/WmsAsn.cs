using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Common.Entities
{
  [Table("wms_asn")]
  public class WmsAsn
  {
    [Key]
    [Column("ID")]
    public long Id { get; private set; }

    [Column("FROM_NAME")]
    public string FromName { get; private set; }
  }
}
