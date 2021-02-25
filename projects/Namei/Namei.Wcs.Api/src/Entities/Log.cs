using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Wcs.Api
{
  [Table("logs")]
  public class Log
  {
    public int id { get; set; }

    public string key { get; set; }

    public string type { get; set; }

    public string message { get; set; }  

    public DateTime created_at { get; set; } = DateTime.Now;
  }
}
