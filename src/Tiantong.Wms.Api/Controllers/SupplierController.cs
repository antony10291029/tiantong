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

    public SupplierController(
      IAuth auth,
      SupplierRepository suppliers,
      WarehouseRepository warehouses
    ) {
      _auth = auth;
      _suppliers = suppliers;
      _warehouses = warehouses;
    }

    public class SupplierCreateParams
    {
      [Required]
      public int? warehouse_id { get; set; }

      [Required]
      public string name { get; set; }

      public string comment { get; set; } = "";

      public bool is_enabled { get; set; } = true;
    }

    public object Create([FromBody] SupplierCreateParams param)
    {
      _auth.EnsureOwner();
      var warehouseId = (int) param.warehouse_id;
      _warehouses.EnsureOwner(warehouseId, _auth.User.id);
      _suppliers.EnsureNameUnique(warehouseId, param.name);

      var supplier = new Supplier {
        warehouse_id = warehouseId,
        name = param.name,
        comment = param.comment,
        is_enabled = param.is_enabled,
      };
      _suppliers.Add(supplier);
      _suppliers.UnitOfWork.SaveChanges();

      return new {
        message = "Success to create Supplier",
        id = supplier.id
      };
    }

    public class SupplierDeleteParams
    {
      [Required]
      public int? id { get; set; }
    }

    public object Delete([FromBody] SupplierDeleteParams param)
    {
      _auth.EnsureOwner();
      var supplier = _suppliers.EnsureGetByOwner((int) param.id, _auth.User.id);
      _suppliers.Remove(supplier.id);
      _suppliers.UnitOfWork.SaveChanges();

      return JsonMessage("Success to delete supplier");
    }

    public class SupplierUpdateParams
    {
      [Required]
      public int? id { get; set; }

      public string name { get; set; }

      public string comment { get; set; }

      public bool? is_enabled { get; set; }
    }

    public object Update([FromBody] SupplierUpdateParams param)
    {
      _auth.EnsureOwner();
      var supplier = _suppliers.EnsureGetByOwner((int) param.id, _auth.User.id);

      if (param.name != null) {
        _suppliers.EnsureNameUnique(supplier.warehouse_id, param.name);
        supplier.name = param.name;
      }
      if (param.comment != null) supplier.comment = param.comment;
      if (param.is_enabled != null) {
        supplier.is_enabled = (bool) param.is_enabled;
      }
      _suppliers.UnitOfWork.SaveChanges();

      return JsonMessage("Success to update supplier");
    }

    public class SupplierSearchParams
    {
      [Nonzero]
      public int warehouse_id { get; set; }

      public int page { get; set; } = 1;

      public int page_size { get; set; } = 10;

      public string search { get; set; }
    }

    public IPagination<Supplier> Search([FromBody] SupplierSearchParams param)
    {
      _auth.EnsureOwner();
      _warehouses.EnsureOwner(param.warehouse_id, _auth.User.id);

      var query = _suppliers.Table.Where(supplier =>
        supplier.warehouse_id == param.warehouse_id &&
        (param.search == null ? true : supplier.name.Contains(param.search))
      );

      return query.OrderBy(supplier => supplier.created_at)
        .Paginate(param.page, param.page_size);;
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
