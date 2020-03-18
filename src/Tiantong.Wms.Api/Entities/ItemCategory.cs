using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("item_categories")]
  public class ItemCategory : Entity
  {
    [Key]
    public int id { get; set; }

    public int warehouse_id { get; set; }

    public string number { get; set; }

    public string name { get; set; }

    public string comment { get; set; } = "";

    public bool is_enabled { get; set; } = true;

  }
}
