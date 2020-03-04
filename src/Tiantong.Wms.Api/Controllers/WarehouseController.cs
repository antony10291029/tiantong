using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class WarehouseController : BaseController
  {
    private IAuth _auth;

    private WarehouseRepository _warehouses;

    public WarehouseController(IAuth auth, WarehouseRepository warehouses)
    {
      _auth = auth;
      _warehouses = warehouses;
    }

    public class CreateWarehouseParams
    {
      [Required]
      [MaxLength(20)]
      [WarehouseNumberUnique]
      public string number { get; set; }

      [MaxLength(20)]
      public string name { get; set; } = "";

      [MaxLength(255)]
      public string address { get; set; } = "";

      [MaxLength(255)]
      public string comment { get; set; } = "";
    }

    public object Create([FromBody] CreateWarehouseParams param)
    {
      _auth.EnsureOwner();
      var warehouse = new Warehouse();
      warehouse.owner_user_id = _auth.User.id;
      warehouse.number = param.number;
      warehouse.name = param.name;
      warehouse.address = param.address;
      warehouse.comment = param.comment;
      _warehouses.Add(warehouse);
      _warehouses.UnitOfWork.SaveChanges();

      return JsonMessage("Success to create warehouse");
    }

    public class UpdateWarehouseParams
    {
      [WarehouseOwnerCheck]
      public int id { get; set; }

      [MaxLength(20)]
      [WarehouseNumberUnique]
      public string number { get; set; }

      [MaxLength(20)]
      public string name { get; set; }

      [MaxLength(255)]
      public string address { get; set; }

      [MaxLength(255)]
      public string comment { get; set; }

      public bool? is_enabled { get; set; }
    }

    public object Update([FromBody] UpdateWarehouseParams param)
    {
      var warehouse = _warehouses.Get(param.id);
      if (param.name != null) warehouse.name = param.name;
      if (param.number != null) warehouse.number = param.number;
      if (param.address != null) warehouse.address = param.address;
      if (param.comment != null) warehouse.comment = param.comment;
      if (param.is_enabled != null) {
        warehouse.is_enabled = (bool) param.is_enabled;
      }
      _warehouses.UnitOfWork.SaveChanges();

      return JsonMessage("Success to update warehouse");
    }

    public Warehouse[] Search()
    {
      _auth.EnsureOwner();

      return _warehouses.Search(_auth.User.id);
    }
  }
}
