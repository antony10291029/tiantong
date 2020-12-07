using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Wcs.Api
{
  [Table("devices")]
  public class Device
  {
    public int id { get; set; }

    public string key { get; set; }

    public string name { get; set; }

    public string type { get; set; }

    public Boolean is_enable { get; set; }

    public DateTime created_at { get; set; }
  }
}
