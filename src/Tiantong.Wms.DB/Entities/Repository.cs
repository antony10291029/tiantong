using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.DB
{
  [Table("repositories")]
  public class Repository
  {
    [Key]
    public int id { get; set; }

    public string name { get; set; }

    public DateTime created_at { get; set; }

    public Repository()
    {
      created_at = DateTime.Now;
    }
  }
}
