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

    public Item EnsureGet(int id)
    {
      var item = Get(id);

      if (item == null) {
        throw new HttpException("User id does not exist");
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
        throw new HttpException("Item ids do not exist in this warehouse");
      }
    }

    public void EnsureNumberUnique(int warehouseId, string number)
    {
      if (HasNumber(warehouseId, number)) {
        throw new HttpException("Item number already exists in this warehouse");
      }
    }
  }
}
