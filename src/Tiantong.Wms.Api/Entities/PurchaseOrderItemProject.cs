using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("purchase_order_item_projects")]
  public class PurchaseOrderItemProject : Entity
  {
    public virtual int order_item_id { get; set; }

    public virtual int project_id { get; set; }

    public virtual int index { get; set; }

    public virtual int quantity { get; set; }

  }
}
