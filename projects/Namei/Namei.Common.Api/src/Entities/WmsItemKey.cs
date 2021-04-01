using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Common.Entities
{
  [Table("wms_item_key")]
  public class WmsItemKey
  {
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("CODE")]
    public string Code { get; set; }

    [Column("LOT_INFO_ERP_CODE")]
    public string LotInfoErpCode { get; set; }
  }
}
