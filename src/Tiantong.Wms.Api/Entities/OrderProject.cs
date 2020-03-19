using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("order_projects")]
  public class OrderProject : Entity
  {
    [Key]
    public int id { get; set; }

    public int warehouse_id { get; set; }

    public int order_id { get; set; }

    public int project_id { get; set; }

    public int key { get; set; }

    public int[] order_item_ids { get; set; } = new int[] {};
  }
}
