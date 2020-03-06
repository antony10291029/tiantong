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

    public class WarehouseCreateParams
    {
      public string number { get; set; } = "";

      public string name { get; set; } = "";

      public string address { get; set; } = "";

      public string comment { get; set; } = "";

      public bool is_enabled { get; set; } = true;
    }

    public object Create([FromBody] WarehouseCreateParams param)
    {
      _auth.EnsureOwner();
      var warehouse = new Warehouse {
        owner_user_id = _auth.User.id,
        number = param.number,
        name = param.name,
        address = param.address,
        comment = param.comment,
        is_enabled = param.is_enabled,
      };
      _warehouses.Add(warehouse);
      _warehouses.UnitOfWork.SaveChanges();

      return new {
        message = "Success to create warehouse",
        id = warehouse.id
      };
    }

    public class WarehouseDeleteParams
    {
      [Required]
      public int? id { get; set; }
    }

    public object Delete([FromBody] WarehouseDeleteParams param)
    {
      _auth.EnsureOwner();
      var warehouseId = (int) param.id;
      _warehouses.EnsureOwner(warehouseId, _auth.User.id);
      _warehouses.Remove(warehouseId);
      _warehouses.UnitOfWork.SaveChanges();

      return JsonMessage("Success to delete warehouse");
    }

    public class WarehouseUpdateParams
    {
      [Required]
      public int? id { get; set; }

      public string number { get; set; }

      public string name { get; set; }

      public string address { get; set; }

      public string comment { get; set; }

      public bool? is_enabled { get; set; }
    }

    public object Update([FromBody] WarehouseUpdateParams param)
    {
      _auth.EnsureOwner();
      var warehouse = _warehouses.EnsureGetByOwner((int) param.id, _auth.User.id);

      if (param.number != null) warehouse.number = param.number;
      if (param.name != null) warehouse.name = param.name;
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
