using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class OrderCategoryRepository : Repository<OrderCategory, int>
  {
    private WarehouseRepository _warehouses;

    public OrderCategoryRepository(DbContext db, WarehouseRepository warehouses) : base(db)
    {
      _warehouses = warehouses;
    }

    //

    public OrderCategory[] Search(int warehouseId)
    {
      return Table.Where(category => category.warehouse_id == warehouseId)
        .OrderBy(category => category.id)
        .ToArray();
    }

    public bool HasName(int warehouseId, string type, string name)
    {
      return Table.Any(ct => ct.warehouse_id == warehouseId && ct.type == type && ct.name == name);
    }

    public OrderCategory EnsureGet(int id)
    {
      var category = Get(id);

      if (category == null) {
        throw new HttpException("Order category id does not exist");
      }

      return category;
    }

    public OrderCategory EnsureGetByOwner(int id, int userId)
    {
      var category = EnsureGet(id);
      _warehouses.EnsureOwner(category.warehouse_id, userId);

      return category;
    }

    public void EnsureNameUnique(int warehouseId, string type, string name)
    {
      if (HasName(warehouseId, type, name)) {
        throw new HttpException("Order category name already exists in this warehouse");
      }
    }
  }
}
