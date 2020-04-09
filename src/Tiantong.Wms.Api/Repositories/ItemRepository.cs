using System.Globalization;
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

    public void Ensure(int warehouseId, int goodId, int itemId)
    {
      if (!Table.Any(item => item.good_id == goodId && item.id == itemId)) {
        throw new FailureOperation("物品规格不存在");
      }
    }

    public Item EnsureGet(int id)
    {
      var item = Get(id);

      if (item == null) {
        throw new FailureOperation("规格未找到");
      }

      return item;
    }

    public void EnsureUnique(Item item)
    {
      if (
        item.id == 0 ? Table.Any(i =>
          i.number == item.number &&
          i.warehouse_id == item.warehouse_id
        ) : Table.Any(i =>
          i.id != item.id &&
          i.number == item.number &&
          i.warehouse_id == item.warehouse_id
        )
      ) {
        throw new FailureOperation("该规格编码已存在");
      }
      if (
        item.id == 0 ? Table.Any(i =>
          i.name == item.name &&
          i.unit == item.unit &&
          i.good_id == item.good_id &&
          i.warehouse_id == item.warehouse_id
        ) : Table.Any(i =>
          i.id != item.id &&
          i.unit == item.unit &&
          i.name == item.name &&
          i.good_id == item.good_id &&
          i.warehouse_id == item.warehouse_id
        )
      ) {
        throw new FailureOperation("该规格名称和单位已存在");
      }
    }

    public void EnsureNumberUnique(int warehouseId, string[] numbers)
    {
      if (numbers.Length > 0 && Table.Any(item => numbers.Contains(item.number))) {
        throw new FailureOperation("规格码重复");
      }
    }

    public void EnsureNameUnique(int goodId, string name)
    {
      if (Table.Any(item => item.good_id == goodId && item.name == name)) {
        throw new FailureOperation("规格名已存在");
      }
    }

  }
}
