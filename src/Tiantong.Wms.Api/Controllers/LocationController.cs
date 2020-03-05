using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class LocationController : BaseController
  {
    private IAuth _auth;

    private AreaRepository _areas;

    private LocationRepository _locations;

    private WarehouseRepository _warehouses;

    public LocationController(
      IAuth auth,
      AreaRepository areas,
      LocationRepository locations,
      WarehouseRepository warehouses
    ) {
      _auth = auth;
      _areas = areas;
      _locations = locations;
      _warehouses = warehouses;
    }

    public class LocationCreateParams
    {
      public int area_id { get; set; }

      public string number { get; set; }

      public string name { get; set; } = "";

      public string comment { get; set; } = "";

      public string total_area { get; set; } = "";

      public string total_volume { get; set; } = "";

      public bool is_enabled { get; set; } = true;
    }

    public object Create([FromBody] LocationCreateParams param)
    {
      _auth.EnsureOwner();
      _locations.EnsureNumberUnique(param.area_id, param.number);
      var area = _areas.EnsureGetByOwner(param.area_id, _auth.User.id);
      _locations.Add(new Location() {
        warehouse_id = area.warehouse_id,
        area_id = area.id,
        number = param.number,
        name = param.name,
        comment = param.comment,
        total_area = param.total_area,
        total_volume = param.total_volume,
        is_enabled = param.is_enabled,
      });
      _locations.UnitOfWork.SaveChanges();

      return JsonMessage("Success to create location");
    }

    public class LocationDeleteParams
    {
      public int id { get; set; }
    }

    public object Delete([FromBody] LocationDeleteParams param)
    {
      _auth.EnsureOwner();
      _locations.EnsureGetByOwner(param.id, _auth.User.id);
      _locations.Remove(param.id);
      _locations.UnitOfWork.SaveChanges();

      return JsonMessage("Success to delete location");
    }

    public class LocationUpdateParams
    {
      public int id { get; set; }

      public string number { get; set; }

      public string name { get; set; }

      public string comment { get; set; }

      public string total_area { get; set; }

      public string total_volume { get; set; }

      public bool? is_enabled { get; set; }
    }

    public object Update([FromBody] LocationUpdateParams param)
    {
      _auth.EnsureOwner();
      var location = _locations.EnsureGetByOwner(param.id, _auth.User.id);

      if (param.name != null) location.name = param.name;
      if (param.comment != null) location.comment = param.comment;
      if (param.total_area != null) location.total_area = param.total_area;
      if (param.total_volume != null) location.total_volume = param.total_volume;
      if (param.is_enabled != null) {
        location.is_enabled = (bool) param.is_enabled;
      }
      if (param.number != null) {
        _locations.EnsureNumberUnique(location.area_id, param.number);
        location.number = param.number;
      }
      _locations.UnitOfWork.SaveChanges();

      return JsonMessage("Success to update location");
    }

    public class LocationSearchParams
    {
      public int warehouse_id { get; set; }
    }

    public Location[] Search([FromBody] LocationSearchParams param)
    {
      _auth.EnsureOwner();
      _warehouses.EnsureOwner(param.warehouse_id, _auth.User.id);

      return _locations.Search(param.warehouse_id);
    }
  }
}
