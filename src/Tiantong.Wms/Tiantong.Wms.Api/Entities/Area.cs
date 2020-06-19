using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Tiantong.Wms.Api
{
  [Table("areas")]
  public class Area : Entity
  {
    public int warehouse_id { get; set; }

    public string number { get; set; } = null;

    public string name { get; set; } = "";

    public string comment { get; set; } = "";

    public string total_area { get; set; } = "";

    public bool is_enabled { get; set; } = true;

    [JsonIgnore]
    [ForeignKey("area_id")]
    public List<Location> locations { get; set; }
  }
}
