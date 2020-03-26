using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class WarehouseController : BaseController
  {
    private IAuth _auth;

    private AreaRepository _areas;

    private LocationRepository _locations;

    private SupplierRepository _suppliers;

    private WarehouseRepository _warehouses;

    private GoodCategoryRepository _goodCategories;

    public WarehouseController(
      IAuth auth,
      AreaRepository areas,
      LocationRepository locations,
      SupplierRepository suppliers,
      WarehouseRepository warehouses,
      GoodCategoryRepository goodCategories
    ) {
      _auth = auth;
      _areas = areas;
      _locations = locations;
      _suppliers = suppliers;
      _warehouses = warehouses;
      _goodCategories = goodCategories;
    }

    public class WarehouseCreateParams
    {
      public string number { get; set; } = null;

      public string name { get; set; } = "";

      public string address { get; set; } = "";

      public string comment { get; set; } = "";

      public bool is_enabled { get; set; } = true;
    }

    public object Create([FromBody] WarehouseCreateParams param)
    {
      _auth.EnsureOwner();
      _warehouses.UnitOfWork.BeginTransaction();

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

      var area = _areas.Add(new Area {
        warehouse_id = warehouse.id,
        name = "默认区域",
      });
      _warehouses.UnitOfWork.SaveChanges();

      _locations.Add(new Location {
        warehouse_id = warehouse.id,
        area_id = area.id, 
        name = "默认位置"
      });
      _suppliers.Add(new Supplier {
        warehouse_id = warehouse.id,
        name = "默认供应商",
      });
      _goodCategories.Add(new GoodCategory {
        warehouse_id = warehouse.id,
        name = "默认货类"
      });
      _warehouses.UnitOfWork.SaveChanges();
      _warehouses.UnitOfWork.Commit();

      return SuccessOperation("仓库已创建", warehouse.id);
    }

    public class WarehouseDeleteParams
    {
      [Nonzero]
      public int warehouse_id { get; set; }
    }

    public object Delete([FromBody] WarehouseDeleteParams param)
    {
      _auth.EnsureOwner();
      _warehouses.EnsureOwner(param.warehouse_id, _auth.User.id);
      _warehouses.Remove(param.warehouse_id);
      _warehouses.UnitOfWork.SaveChanges();

      return JsonMessage("Success to delete warehouse");
    }

    public class WarehouseUpdateParams
    {
      [Nonzero]
      public int id { get; set; }

      public string number { get; set; }

      public string name { get; set; }

      public string address { get; set; }

      public string comment { get; set; }

      public bool? is_enabled { get; set; }
    }

    public object Update([FromBody] WarehouseUpdateParams param)
    {
      _auth.EnsureOwner();
      var warehouse = _warehouses.EnsureGetByOwner(param.id, _auth.User.id);

      if (param.number != null) warehouse.number = param.number;
      if (param.name != null) warehouse.name = param.name;
      if (param.address != null) warehouse.address = param.address;
      if (param.comment != null) warehouse.comment = param.comment;
      if (param.is_enabled != null) {
        warehouse.is_enabled = (bool) param.is_enabled;
      }
      _warehouses.UnitOfWork.SaveChanges();

      return SuccessOperation("仓库设置已保存");
    }

    public Warehouse[] Search()
    {
      _auth.EnsureOwner();

      return _warehouses.Search(_auth.User.id);
    }

    public class FindParams
    {
      [Nonzero]
      public int warehouse_id {get; set; }
    }

    public Warehouse Find([FromBody] FindParams param)
    {
      _auth.EnsureOwner();

      return _warehouses.Find(param.warehouse_id, _auth.User.id);
    }
  }
}
