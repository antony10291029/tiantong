using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class ItemRepository : Repository<Item, int>
  {
    private WarehouseRepository _warehouses;

    public ItemRepository(DbContext db, WarehouseRepository warehouses) : base(db)
    {
      _warehouses = warehouses;
    }

    //

    public Item[] Search(int warehouseId)
    {
      return Table.Where(item => item.warehouse_id == warehouseId)
        .OrderBy(item => item.number)
        .ToArray();
    }

    public bool HasNumber(int warehouseId, string number)
    {
      return Table.Any(item => item.warehouse_id == warehouseId && item.number == number);
    }

    public bool HasCategory(int warehouseId, int categoryId)
    {
      return Table.Any(item => item.warehouse_id == warehouseId && item.category_ids.Contains(categoryId));
    }

    public Item EnsureGet(int id)
    {
      var item = Get(id);

      if (item == null) {
        throw new FailureOperation("货品不存在");
      }

      return item;
    }

    public Item EnsureGetByOwner(int id, int userId)
    {
      var item = EnsureGet(id);
      _warehouses.EnsureOwner(item.warehouse_id, userId);

      return item;
    }

    public void EnsureIds(int warehouseId, int[] ids)
    {
      if (Table.Where(item => ids.Contains(item.id)).Count() != ids.Length) {
        throw new FailureOperation("货品不存在");
      }
    }

    public void EnsureNumberUnique(int warehouseId, string number)
    {
      if (HasNumber(warehouseId, number)) {
        throw new FailureOperation("货品编码重复");
      }
    }
  }
}
