using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class WarehouseController : BaseController
  {
    private Auth _auth;

    private AreaRepository _areas;

    private LocationRepository _locations;

    private SupplierRepository _suppliers;

    private WarehouseRepository _warehouses;

    private DepartmentRepository _departments;

    private GoodCategoryRepository _goodCategories;

    private WarehouseUserRepository _warehouseUsers;

    public WarehouseController(
      Auth auth,
      AreaRepository areas,
      LocationRepository locations,
      SupplierRepository suppliers,
      WarehouseRepository warehouses,
      DepartmentRepository departments,
      GoodCategoryRepository goodCategories
    ) {
      _auth = auth;
      _areas = areas;
      _locations = locations;
      _suppliers = suppliers;
      _warehouses = warehouses;
      _departments = departments;
      _goodCategories = goodCategories;
      _warehouseUsers = warehouses.Users;
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
      var warehouse = new Warehouse {
        number = param.number,
        name = param.name,
        address = param.address,
        comment = param.comment,
        is_enabled = param.is_enabled,
      };
      _warehouses.Add(warehouse);

      return SuccessOperation("仓库已创建", warehouse.id);
    }
 
    public class WarehouseDeleteParams
    {
      [Nonzero]
      public int warehouse_id { get; set; }
    }

    public object Delete([FromBody] WarehouseDeleteParams param)
    {
      _auth.EnsureUser();
      _warehouses.EnsureUser(param.warehouse_id, _auth.User.id);
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
      _auth.EnsureUser();
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
      _auth.EnsureUser();

      return _warehouses.Search(_auth.User.id);
    }

    public class FindParams
    {
      [Nonzero]
      public int warehouse_id {get; set; }
    }

    public Warehouse Find([FromBody] FindParams param)
    {
      _auth.EnsureUser();

      return _warehouses.Find(param.warehouse_id, _auth.User.id);
    }
  }
}
