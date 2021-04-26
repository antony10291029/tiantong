using Midos.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Common.Entities
{
  [Table("wms_location")]
  public class WmsLocation: IEntity
  {
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("CODE")]
    public string Code { get; set; }
  }
}
