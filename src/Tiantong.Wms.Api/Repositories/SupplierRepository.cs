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

    public Supplier[] Search(int warehouseId)
    {
      return Table.Where(supplier => supplier.warehouse_id == warehouseId)
        .OrderBy(supplier => supplier.created_at)
        .ToArray();
    }

    public bool HasName(int warehouseId, string name)
    {
      return Table.Any(Supplier =>
        Supplier.warehouse_id == warehouseId &&
        Supplier.name == name
      );
    }

    public Supplier EnsureGet(int id)
    {
      var supplier = Get(id);

      if (supplier == null) {
        throw new HttpException("Supplier id does not exist");
      }

      return supplier;
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
        throw new HttpException("Supplier name already exists in this warehouse");
      }
    }
  }
}
