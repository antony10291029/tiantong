using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.DB
{
  [Table("projects")]
  public class Project
  {
    [Key]
    public int id { get; set; }

    public string number { get; set; }

    public int[] repository_ids { get; set; }

    public DateTime created_at { get; set; }

    public DateTime? finished_at { get; set; }

    public Project()
    {
      created_at = DateTime.Now;
    }
  }
}
