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
      public int warehouse_id { get; set; }

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
      _warehouses.EnsureOwner(param.warehouse_id, _auth.User.id);
      _areas.EnsureNumberUnique(param.warehouse_id, param.number);

      var area = new Area();
      area.name = param.name;
      area.number = param.number;
      area.comment = param.comment;
      area.total_area = param.total_area;
      area.is_enabled = param.is_enabled;
      area.warehouse_id = param.warehouse_id;
      _areas.Add(area);
      _areas.UnitOfWork.SaveChanges();

      return JsonMessage("Success to create aera");
    }

    public class AreaDeleteParams
    {
      public int id { get; set; }
    }

    public object Delete([FromBody] AreaDeleteParams param)
    {
      _auth.EnsureOwner();
      _areas.EnsureGetByOwner(param.id, _auth.User.id);
      _areas.Remove(param.id);
      _areas.UnitOfWork.SaveChanges();

      return JsonMessage("Success to delete area");
    }

    public class AreaUpdateParams
    {
      public int id { get; set; }

      public string number { get; set; }

      public string name { get; set; }

      public string comment { get; set; }

      public bool? is_enabled { get; set; }
    }

    public object Update([FromBody] AreaUpdateParams param)
    {
      _auth.EnsureOwner();
      var area = _areas.EnsureGetByOwner(param.id, _auth.User.id);

      if (param.name != null) area.name = param.name;
      if (param.comment != null) area.comment = param.comment;
      if (param.is_enabled != null) {
        area.is_enabled = (bool) param.is_enabled;
      }
      if (param.number != null) {
        _areas.EnsureNumberUnique(area.warehouse_id, param.number);
        area.number = param.number;
      }

      _areas.UnitOfWork.SaveChanges();

      return JsonMessage("Success to update area");
    }

    public class AreaSearchParams
    {
      public int warehouse_id { get; set; }
    }

    public Area[] Search([FromBody] AreaSearchParams param)
    {
      _auth.EnsureOwner();
      _warehouses.EnsureOwner(param.warehouse_id, _auth.User.id);

      return _areas.Search(param.warehouse_id);
    }
  }
}
