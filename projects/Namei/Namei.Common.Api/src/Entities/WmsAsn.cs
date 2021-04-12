using Midos.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Common.Entities
{
  [Table("wms_asn")]
  public class WmsAsn: IEntity
  {
    [Key]
    [Column("ID")]
    public long Id { get; private set; }

    [Column("FROM_NAME")]
    public string FromName { get; private set; }

    [Column("CUSTOMER_BILL")]
    public string CUSTOMER_BILL { get; private set; }

    [Column("BILL_TYPE_ID")]
    public long BillTypeId { get; private set; }
  }
}
