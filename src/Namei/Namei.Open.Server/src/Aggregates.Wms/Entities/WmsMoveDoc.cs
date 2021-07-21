using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Open.Server
{
  [Table("wms_move_doc")]
  public class WmsMoveDoc
  {
    [Column("ID")]
    public long Id{ get; set; }

    [Column("CODE")]
    public string Code { get; set; }

    [Column("RELATED_BILL_ID")]
    public long RelatedBillId { get; set; }
  }
}
