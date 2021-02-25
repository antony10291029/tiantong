using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Wcs.Api
{
  [Table("devices")]
  public class Device
  {
    public int id { get; set; }

    [Required(ErrorMessage = "设备代码不能为空")]
    public string key { get; set; }

    [Required(ErrorMessage = "设备名称不能为空")]
    public string name { get; set; }

    public string type { get; set; }

    public Boolean is_enable { get; set; }

    public DateTime created_at { get; set; }
  }
}
