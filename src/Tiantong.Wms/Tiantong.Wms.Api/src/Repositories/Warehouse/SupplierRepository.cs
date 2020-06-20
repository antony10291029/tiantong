using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class SupplierRepository : Repository<Supplier, int>
  {
    private BaseOrderRepository _orders;

    public SupplierRepository(
      DbContext db,
      BaseOrderRepository orders
    ) : base(db) {
      _orders = orders;
    }

    //

    public bool IsRemovable(Supplier supplier)
    {
      return !_orders.HasSupplier(supplier.id);
    }

    public override bool Remove(Supplier supplier)
    {
      if (!IsRemovable(supplier)) {
        throw new FailureOperation("供应商已使用，无法删除");
      }

      DbContext.Remove(supplier);

      return true;
    }

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

    public void EnsureExists(int warehouseId, int supplierId)
    {
      if (!
        Table.Any(supplier =>
          supplier.id == supplierId &&
          supplier.warehouse_id == warehouseId
        )
      ) {
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

    public void EnsureNameUnique(int warehouseId, string name)
    {
      if (HasName(warehouseId, name)) {
        throw new FailureOperation("供应商名称不可重复");
      }
    }
  }
}
