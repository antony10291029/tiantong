using System.ComponentModel.DataAnnotations.Schema;

namespace Yuchuan.IErp.Api
{
  [Table("devices")]
  public class Device
  {
    public int id { get; set; }

    public string name { get; set; }

    public string region { get; set; }

    public string province { get; set; }

    public string city { get; set; }

    public string comment { get; set; }

    public bool is_enabled { get; set; }
  }
}
