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
      [MaxLength(20)]
      public string number { get; set; } = "";

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
      warehouse.name = param.name;
      warehouse.number = param.number;
      warehouse.address = param.address;
      warehouse.comment = param.comment;
      warehouse.owner_user_id = _auth.User.id;
      _warehouses.Add(warehouse);
      _warehouses.UnitOfWork.SaveChanges();

      return JsonMessage("Success to create warehouse");
    }

    public class UpdateWarehouseParams
    {
      [Required]
      public int id { get; set; }

      [MaxLength(20)]
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
      _auth.EnsureOwner();
      var warehouse = _warehouses.EnsureGetByOwner(param.id, _auth.User.id);
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
