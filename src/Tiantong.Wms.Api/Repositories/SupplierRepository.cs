using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class SupplierRepository : Repository<Supplier, int>
  {
    private WarehouseRepository _warehouses;

    public SupplierRepository(DbContext db, WarehouseRepository warehouses) : base(db)
    {
      _warehouses = warehouses;
    }

    //

    public bool HasId(int warehouseId, int id)
    {
      return Table.Any(supplier => supplier.warehouse_id == warehouseId && supplier.id == id);
    }

    public bool HasName(int warehouseId, string name)
    {
      return Table.Any(Supplier =>
        Supplier.warehouse_id == warehouseId &&
        Supplier.name == name
      );
    }

    public void Ensure(int warehouseId, int id)
    {
      if (!Table.Any(supplier => supplier.warehouse_id == warehouseId && supplier.id == id)) {
        throw new FailureOperation("供应商不存在");
      }
    }

    public Supplier EnsureGet(int id)
    {
      var supplier = Get(id);

      if (supplier == null) {
        throw new FailureOperation("供应商不存在");
      }

      return supplier;
    }

    public void EnsureId(int warehouseId, int id)
    {
      if (!HasId(warehouseId, id)) {
        throw new FailureOperation("无法在仓库中找到该供应商");
      }
    }

    public Supplier EnsureGetByOwner(int id, int userId)
    {
      var supplier = EnsureGet(id);
      _warehouses.EnsureOwner(supplier.warehouse_id, userId);

      return supplier;
    }

    public void EnsureNameUnique(int warehouseId, string name)
    {
      if (HasName(warehouseId, name)) {
        throw new FailureOperation("供应商名称不可重复");
      }
    }
  }
}
