using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("order_projects")]
  public class OrderProject : Entity
  {
    public int key { get; set; }

    public int order_id { get; set; }

    public int project_id { get; set; }

    public int[] order_item_ids { get; set; } = new int[] {};
  }
}
