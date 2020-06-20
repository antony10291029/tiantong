using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Tiantong.Wms.Api
{
  [Table("warehouse_users")]
  public class WarehouseUser : Entity
  {
    public virtual int warehouse_id { get; set; }

    public virtual int department_id { get; set; }

    public virtual int user_id { get; set; }

    public virtual bool is_accepted { get; set; } = false;

    [JsonIgnore]
    [ForeignKey("warehouse_id")]
    public virtual Warehouse warehouse { get; set; }

    [ForeignKey("user_id")]
    public virtual User user { get; set; }

    [JsonIgnore]
    [ForeignKey("department_id")]
    public virtual Department department { get; set; }
  }
}
