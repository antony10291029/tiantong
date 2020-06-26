using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yuchuan.IErp.Api
{
  [Table("projects")]
  public class Project
  {
    public int id { get; set; }

    public string name { get; set; }

    public bool is_enabled { get; set; }

    public DateTime created_at { get; set; }
  }
}
