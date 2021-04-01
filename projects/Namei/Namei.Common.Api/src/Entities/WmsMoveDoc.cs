using System.Collections.Generic;
using Midos.Domain;
using System;
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

    [Column("RELATED_BILL_ID")]
    public long RelatedBillId { get; set; }

    [Column("TYPE")]
    public string Type { get; set; }

    public List<WmsTask> Tasks { get; set; }
  }
}
