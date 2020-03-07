using System;
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
      [Required]
      public int? warehouse_id { get; set; }
    }

    public Supplier[] Search([FromBody] SupplierSearchParams param)
    {
      _auth.EnsureOwner();
      var warehouseId = (int) param.warehouse_id;
      _warehouses.EnsureOwner(warehouseId, _auth.User.id);

      return _suppliers.Search(warehouseId);
    }
  }
}
