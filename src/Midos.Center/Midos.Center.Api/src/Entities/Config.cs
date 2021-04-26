using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Midos.Center.Entities
{
  [Table("configs")]
  public class Config
  {
    [Key]
    [Column("key")]
    public string Key { get; set; }

    [Column("value")]
    public string Value { get; set; }


    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
  }
}
