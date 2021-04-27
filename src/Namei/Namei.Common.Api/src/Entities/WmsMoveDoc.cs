using System.Collections.Generic;
using Midos.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Common.Entities
{
  [Table("wms_move_doc")]
  public class WmsMoveDoc: IEntity
  {
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [ForeignKey("PickTicket")]
    [Column("RELATED_BILL_ID")]
    public long? RelatedBillId { get; set; }

    [Column("CODE")]
    public string Code { get; set; }

    [Column("TYPE")]
    public string Type { get; set; }

    public WmsPickTicket PickTicket { get; set; }
  }
}
