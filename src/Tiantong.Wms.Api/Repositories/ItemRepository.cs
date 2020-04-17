using System;
using System.Collections.Generic;
using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class ItemRepository : Repository<Item, int>
  {
    private StockRepository _stocks;

    private BaseOrderRepository _orders;

    public ItemRepository(
      DbContext db,
      StockRepository stocks,
      BaseOrderRepository orders
    ) : base(db) {
      _stocks = stocks;
      _orders = orders;
    }

    // Delete

    public bool isRemovable(Item item)
    {
      if (item.id == 0) {
        return true;
      } else {
        return !_orders.HasItem(item.id) &&
          !_stocks.HasItem(item.id);
      }
    }

    public bool isRemovable(Item[] items)
    {
      var ids = items
        .Where(item => item.id != 0)
        .Select(item => item.id)
        .ToArray();

      if (ids.Count() == 0) {
        return true;
      }

      return !_orders.HasItem(ids) &&
        !_orders.HasItem(ids);
    }

    public void Remove(Item[] items)
    {
      if (!isRemovable(items)) {
        throw new FailureOperation("规格已被使用，无法删除");
      } else {
        DbContext.RemoveRange(items);
      }
    }

    // Select

    public void EnsureExists(int warehouseId, int goodId, int itemId)
    {
      if (
        !Table.Any(item =>
          item.id == itemId &&
          item.good_id == goodId &&
          item.warehouse_id == warehouseId
        )
      ) {
        throw new FailureOperation("物品规格不存在");
      }
    }

    public Item EnsureGet(int id)
    {
      var result = Table
        .Where(item => item.id == id)
        .SingleOrDefault();

      if (result == null) {
        throw new FailureOperation("规格不存在");
      }

      return result;
    }

    public void EnsureUnique(Item item)
    {
      if (
        item.number != null && (item.id == 0 ? DbContext.Items.Any(i =>
          i.number == item.number &&
          i.warehouse_id == item.warehouse_id
        ) : DbContext.Items.Any(i =>
          i.id != item.id &&
          i.number == item.number &&
          i.warehouse_id == item.warehouse_id
        ))
      ) {
        throw new FailureOperation("该规格编码已存在");
      }

      if (
        item.id == 0 ? DbContext.Items.Any(i =>
          i.name == item.name &&
          i.unit == item.unit &&
          i.good_id == item.good_id &&
          i.warehouse_id == item.warehouse_id
        ) : DbContext.Items.Any(i =>
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

    public void EnsureUnique(IEnumerable<Item> items)
    {
      if (items.GroupBy(item => item.number).Any(item => item.Key != null && item.Count() > 1)) {
        throw new FailureOperation("规格编码不可重复");
      }

      if (items.GroupBy(item => item.good_id + item.name + item.unit).Any(item => item.Count() > 1)) {
        throw new FailureOperation("规格名称和单位不可重复");
      }

      foreach (var item in items) {
        EnsureUnique(item);
      }
    }

  }
}
