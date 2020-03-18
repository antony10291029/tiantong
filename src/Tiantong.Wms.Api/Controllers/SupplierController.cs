using System.Reflection.Metadata.Ecma335;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class SupplierController : BaseController
  {
    private IAuth _auth;

    private SupplierRepository _suppliers;

    private WarehouseRepository _warehouses;

    private OrderSupplierRepository _orderSuppliers;

    public SupplierController(
      IAuth auth,
      SupplierRepository suppliers,
      WarehouseRepository warehouses,
      OrderSupplierRepository orderSuppliers
    ) {
      _auth = auth;
      _suppliers = suppliers;
      _warehouses = warehouses;
      _orderSuppliers = orderSuppliers;
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
      _auth.EnsureOwner();
      _warehouses.EnsureOwner(param.warehouse_id, _auth.User.id);
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

    public class SupplierDeleteParams
    {
      [Nonzero]
      public int supplier_id { get; set; }
    }

    public object Delete([FromBody] SupplierDeleteParams param)
    {
      _auth.EnsureOwner();

      if (_orderSuppliers.HasSupplier(param.supplier_id)) {
        return FailureOperation("供应商已使用，无法被删除");
      } else {
        _suppliers.Remove(param.supplier_id);
        _suppliers.UnitOfWork.SaveChanges();

        return SuccessOperation("供应商已删除");
      }
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
      _auth.EnsureOwner();
      var supplier = _suppliers.EnsureGetByOwner(param.id, _auth.User.id);

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

    public class SupplierSearchParams
    {
      [Nonzero]
      public int warehouse_id { get; set; }

      public int page { get; set; }

      public int page_size { get; set; }

      public string search { get; set; }
    }

    public IPagination<Supplier> Search([FromBody] SupplierSearchParams param)
    {
      _auth.EnsureOwner();
      _warehouses.EnsureOwner(param.warehouse_id, _auth.User.id);

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
      _auth.EnsureOwner();
      _warehouses.EnsureOwner(param.warehouse_id, _auth.User.id);
      _suppliers.EnsureId(param.warehouse_id, param.supplier_id);

      return _suppliers.EnsureGet(param.supplier_id);
    }
  }
}
