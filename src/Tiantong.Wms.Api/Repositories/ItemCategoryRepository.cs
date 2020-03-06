using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class ItemCategoryRepository : Repository<ItemCategory, int>
  {
    private WarehouseRepository _warehouses;

    public ItemCategoryRepository(DbContext db, WarehouseRepository warehouses) : base(db)
    {
      _warehouses = warehouses;
    }

    //

    public ItemCategory[] Search(int warehouseId)
    {
      return Table.Where(category => category.warehouse_id == warehouseId)
        .OrderBy(category => category.id)
        .ToArray();
    }

    public bool HasIds(int wId, int[] ids)
    {
      var count = Table.Where(ct => ct.warehouse_id == wId && ids.Contains(ct.id)).Count();

      return count == ids.Length;
    }

    public bool HasName(int warehouseId, string name)
    {
      return Table.Any(category => category.warehouse_id == warehouseId && category.name == name);
    }

    public ItemCategory EnsureGet(int id)
    {
      var category = Get(id);

      if (category == null) {
        throw new HttpException("Item Category id does not exist");
      }

      return category;
    }

    public void EnsureIds(int warehouseId, int[] ids)
    {
      if (!HasIds(warehouseId, ids)) {
        throw new HttpException("Item Category id does exist");
      }
    }

    public ItemCategory EnsureGetByOwner(int id, int userId)
    {
      var category = EnsureGet(id);
      _warehouses.EnsureOwner(category.warehouse_id, userId);

      return category;
    }

    public void EnsureNameUnique(int warehouseId, string name)
    {
      if (HasName(warehouseId, name)) {
        throw new HttpException("Item category name already exists in this warehouse");
      }
    }
  }
}
