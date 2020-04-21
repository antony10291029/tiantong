using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class SupplierController : BaseController
  {
    private Auth _auth;

    private SupplierRepository _suppliers;

    private WarehouseRepository _warehouses;

    public SupplierController(
      Auth auth,
      SupplierRepository suppliers,
      WarehouseRepository warehouses
    ) {
      _auth = auth;
      _suppliers = suppliers;
      _warehouses = warehouses;
    }

    public class SupplierCreateParams
    {
      [Nonzero]
      public int warehouse_id { get; set; }

      [Required]
      public string name { get; set; }

      public string comment { get; set; } = "";

      public bool is_enabled { get; set; } = true;
    }

    public object Create([FromBody] SupplierCreateParams param)
    {
      _auth.EnsureUser();
      _warehouses.EnsureUser(param.warehouse_id, _auth.User.id);
      _suppliers.EnsureNameUnique(param.warehouse_id, param.name);

      var supplier = new Supplier {
        warehouse_id = param.warehouse_id,
        name = param.name,
        comment = param.comment,
        is_enabled = param.is_enabled,
      };
      _suppliers.Add(supplier);
      _suppliers.UnitOfWork.SaveChanges();

      return SuccessOperation("供应商已添加", supplier.id);
    }

    public class RemoveParams
    {
      public int id { get; set; }
    }

    public object Remove([FromBody] RemoveParams param)
    {
      var supplier = _suppliers.EnsureGet(param.id);
      _warehouses.EnsureUser(supplier.warehouse_id, _auth.User.id);

      _suppliers.Remove(supplier);
      _suppliers.UnitOfWork.SaveChanges();

      return SuccessOperation("供应商已删除");
    }

    public class SupplierUpdateParams
    {
      [Nonzero]
      public int id { get; set; }

      public string name { get; set; }

      public string comment { get; set; }

      public bool? is_enabled { get; set; }
    }

    public object Update([FromBody] SupplierUpdateParams param)
    {
      _auth.EnsureUser();
      var supplier = _suppliers.EnsureGet(param.id);
      _warehouses.EnsureUser(supplier.warehouse_id, _auth.User.id);

      if (param.name != null) {
        _suppliers.EnsureNameUnique(supplier.warehouse_id, param.name);
        supplier.name = param.name;
      }
      if (param.comment != null) supplier.comment = param.comment;
      if (param.is_enabled != null) {
        supplier.is_enabled = (bool) param.is_enabled;
      }
      _suppliers.UnitOfWork.SaveChanges();

      return SuccessOperation("供应商信息已保存");
    }

    public class SearchParams : BaseSearchParams
    {
      [Nonzero]
      public int warehouse_id { get; set; }
    }

    public IPagination<Supplier> Search([FromBody] SearchParams param)
    {
      _auth.EnsureUser();
      _warehouses.EnsureUser(param.warehouse_id, _auth.User.id);

      return _suppliers.Table
        .Where(supplier =>
          supplier.warehouse_id == param.warehouse_id &&
          (param.search == null ? true : supplier.name.Contains(param.search))
        )
        .OrderBy(supplier => supplier.created_at)
        .Paginate(param.page, param.page_size);
    }

    public class FindParams
    {
      [Nonzero]
      public int warehouse_id { get; set; }

      [Nonzero]
      public int supplier_id { get; set; }
    }

    public Supplier Find([FromBody] FindParams param)
    {
      _auth.EnsureUser();
      _warehouses.EnsureUser(param.warehouse_id, _auth.User.id);
      _suppliers.EnsureId(param.warehouse_id, param.supplier_id);

      return _suppliers.EnsureGet(param.supplier_id);
    }

    public class AllParams
    {
      public int warehouse_id { get; set; }
    }

    public IEntities<Supplier, int> All([FromBody] AllParams param)
    {
      _auth.EnsureUser();
      _warehouses.EnsureUser(param.warehouse_id, _auth.User.id);

      return _suppliers.Table
        .Where(supplier => supplier.warehouse_id == param.warehouse_id)
        .ToEntities();
    }
  }
}
