using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class AreaController : BaseController
  {
    private IAuth _auth;

    private AreaRepository _areas;

    private WarehouseRepository _warehouses;

    public AreaController(IAuth auth, AreaRepository areas, WarehouseRepository warehouses)
    {
      _auth = auth;
      _areas = areas;
      _warehouses = warehouses;
    }

    public class AreaCreateParams
    {
      [Required]
      public int? warehouse_id { get; set; }

      [Required]
      public string number { get; set; }

      public string name { get; set; } = "";

      public string comment { get; set; } = "";

      public string total_area { get; set; } = "";

      public bool is_enabled { get; set; } = true;
    }

    public object Create([FromBody] AreaCreateParams param)
    {
      _auth.EnsureOwner();
      var warehouseId = (int) param.warehouse_id;
      _warehouses.EnsureOwner(warehouseId, _auth.User.id);
      _areas.EnsureNumberUnique(warehouseId, param.number);

      var area = new Area {
        warehouse_id = warehouseId,
        number = param.number,
        name = param.name,
        comment = param.comment,
        total_area = param.total_area,
        is_enabled = param.is_enabled,
      };
      _areas.Add(area);
      _areas.UnitOfWork.SaveChanges();

      return new {
        message = "Success to create aera",
        id = area.id
      };
    }

    public class AreaDeleteParams
    {
      [Required]
      public int? id { get; set; }
    }

    public object Delete([FromBody] AreaDeleteParams param)
    {
      _auth.EnsureOwner();
      var area = _areas.EnsureGetByOwner((int) param.id, _auth.User.id);
      _areas.Remove(area.id);
      _areas.UnitOfWork.SaveChanges();

      return JsonMessage("Success to delete area");
    }

    public class AreaUpdateParams
    {
      [Required]
      public int? id { get; set; }

      public string number { get; set; }

      public string name { get; set; }

      public string comment { get; set; }

      public string total_area { get; set; }

      public bool? is_enabled { get; set; }
    }

    public object Update([FromBody] AreaUpdateParams param)
    {
      _auth.EnsureOwner();
      var area = _areas.EnsureGetByOwner((int) param.id, _auth.User.id);

      if (param.name != null) area.name = param.name;
      if (param.comment != null) area.comment = param.comment;
      if (param.is_enabled != null) {
        area.is_enabled = (bool) param.is_enabled;
      }
      if (param.total_area != null) area.total_area = param.total_area;
      if (param.number != null) {
        _areas.EnsureNumberUnique(area.warehouse_id, param.number);
        area.number = param.number;
      }

      _areas.UnitOfWork.SaveChanges();

      return JsonMessage("Success to update area");
    }

    public class AreaSearchParams
    {
      [Required]
      public int? warehouse_id { get; set; }
    }

    public Area[] Search([FromBody] AreaSearchParams param)
    {
      _auth.EnsureOwner();
      var warehouseId = (int) param.warehouse_id;
      _warehouses.EnsureOwner(warehouseId, _auth.User.id);

      return _areas.Search(warehouseId);
    }
  }
}
