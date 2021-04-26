using Midos.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Common.Entities
{
  [Table("wms_pick_ticket")]
  public class WmsPickTicket: IEntity
  {
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("RELATED_BILL1")]
    public string RelatedBill1 { get; set; }

    [Column("BILL_TYPE_ID")]
    public string BillTypeId { get; set; }

    [Column("CODE")]
    public string Code { get; set; }
  }
}
