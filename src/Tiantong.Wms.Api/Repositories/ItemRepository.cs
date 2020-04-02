using System.Collections.Generic;
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

    public Item EnsureGet(int id)
    {
      var item = Get(id);

      if (item == null) {
        throw new FailureOperation("规格未找到");
      }

      return item;
    }

    public void EnsureNumberUnique(int warehouseId, string[] numbers)
    {
      if (numbers.Length > 0 && Table.Any(item => numbers.Contains(item.number))) {
        throw new FailureOperation("规格码重复");
      }
    }

  }
}
