using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Common.Entities
{
  [Table("wms_asn_detail")]
  public class WmsAsnDetail
  {
    [Key]
    [Column("ID")]
    public long Id { get; private set; }

    [ForeignKey("Asn")]
    [Column("ASN_ID")]
    public long AsnId { get; private set; }

    [ForeignKey("Item")]
    [Column("ITEM_ID")]
    public long ItemId { get; private set; }

    public WmsAsn Asn { get; private set; }

    public WmsItem Item { get; private set; }
  }
}
