using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("warehouse_users")]
  public class WarehouseUser : Entity
  {
    public virtual int warehouse_id { get; set; }

    public virtual int user_id { get; set; }

    [ForeignKey("user_id")]
    public virtual User user { get; set; }

  }
}
