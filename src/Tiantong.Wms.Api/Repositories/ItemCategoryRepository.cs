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

    public bool HasNumber(int warehouseId, string number)
    {
      return Table.Any(category => category.warehouse_id == warehouseId && category.number == number);
    }

    public ItemCategory EnsureGet(int id)
    {
      var category = Get(id);

      if (category == null) {
        throw new FailureOperation("货类不存在");
      }

      return category;
    }

    public ItemCategory EnsureGet(int id, int warehouseId)
    {
      var category = Get(id);

      if (category == null || category.warehouse_id != warehouseId) {
        throw new FailureOperation("货类不存在");
      }

      return category;
    }

    public void EnsureIds(int warehouseId, int[] ids)
    {
      if (!HasIds(warehouseId, ids)) {
        throw new FailureOperation("货类不存在");
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
        throw new FailureOperation("货类名称已经存在");
      }
    }

    public void EnsureNumberUnique(int warehouseId, string number)
    {
      if (HasNumber(warehouseId, number)) {
        throw new FailureOperation("货类编码已经存在");
      }
    }
  }
}
