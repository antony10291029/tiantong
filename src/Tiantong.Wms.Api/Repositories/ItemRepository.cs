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

    public void EnsureNumberUnique(int warehouseId, string[] numbers)
    {
      if (numbers.Length > 0 && Table.Any(item => numbers.Contains(item.number))) {
        throw new FailureOperation("规格码重复");
      }
    }

    public Dictionary<string, Item> GetByGoods(Good[] goods)
    {
      var ids = goods.SelectMany(good => good.item_ids).Distinct();

      return Table.Where(item => ids.Contains(item.id)).ToRelationship();
    }
  }
}
