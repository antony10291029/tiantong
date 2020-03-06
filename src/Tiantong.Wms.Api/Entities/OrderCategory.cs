using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("order_categories")]
  public class OrderCategory : Entity
  {
    [Key]
    public int id { get; set; }

    public int warehouse_id { get; set; }

    public string type { get; set; }

    public string name { get; set; }

    public string comment { get; set; } = "";

    public bool is_enabled { get; set; } = true;

  }
}
