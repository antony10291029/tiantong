using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Midos.Domain;
using Newtonsoft.Json;

namespace Namei.Wcs.Aggregates
{
  [Table("tcs_map_data")]
  public class TcsMapData
  {
    [Key]
    [Column("map_data_code")]
    public string MapDataCode { get; set; }

    [Column("data_name")]
    public string DataName { get; set; }

    [Column("area_code")]
    public string AreaCode { get; set; }

    [Column("pod_code")]
    public string PodCode { get; set; }

    [Column("coo_x")]
    public double CooX { get; set; }

    [Column("coo_y")]
    public double CooY { get; set; }

    [Column("wcs_area_seq")]
    public int WcsAreaSeq { get; set; }
  }
}
