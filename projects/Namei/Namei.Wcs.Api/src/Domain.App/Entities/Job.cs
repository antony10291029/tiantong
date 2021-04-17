using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Wcs.Api
{
  [Table("jobs")]
  public class Job
  {
    public int id { get; set; }

    public string name { get; set; }

    public int interval { get; set; }

    public bool is_enable { get; set; }

    public int count { get; set; }

    public DateTime executed_at { get; set; } = DateTime.Now;
  }
}
