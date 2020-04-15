using System.Linq;
using Microsoft.EntityFrameworkCore;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class GoodRepository : Repository<Good, int>
  {
    public ItemRepository Items { get; }

    public StockRepository _stocks { get; }

    private OrderRepository _orders { get; }

    public GoodRepository(
      DbContext db,
      ItemRepository items,
      StockRepository stocks,
      OrderRepository orders
    ) : base(db) {
      Items = items;
      _stocks = stocks;
      _orders = orders;
    }

    // Create

    public override Good Add(Good good)
    {
      good.items.ForEach(item => item.warehouse_id = good.warehouse_id);
      EnsureUnique(good);
      Items.EnsureUnique(good.items);
      DbContext.Add(good);

      return good;
    }

    // Delete

    public bool IsRemovable(Good good)
    {
      if (good.id == 0) return true;

      return !_orders.HasGood(good.id) &&
        !_stocks.HasGood(good.id);
    }

    public override bool Remove(Good good)
    {
      var itemIds = good.items.Select(item => item.id).ToArray();

      if (IsRemovable(good)) {
        throw new FailureOperation("该货品已被使用，无法删除");
      }

      Items.Remove(good.items.ToArray());
      DbContext.Remove(good);

      return true;
    }

    // Update

    public override Good Update(Good good)
    {
      Items.EnsureUnique(good.items);

      var oldGood = Table
        .Include(g => g.items)
        .FirstOrDefault(g => g.id == good.id);

      Items.Remove(
        oldGood.items.Where(item =>
          !good.items.Any(i => i.id == item.id)
        ).ToArray()
      );

      foreach (var item in good.items) {
        var oldItem = oldGood.items.Where(i => i.id == item.id).FirstOrDefault();

        if (oldItem == null || item.id == 0) {
          item.warehouse_id = good.warehouse_id;
          oldGood.items.Add(item);
        } else {
          Items.EnsureUnique(item);
          DbContext.Entry(oldItem).CurrentValues.SetValues(item);
        }
      }

      DbContext.Entry(oldGood).CurrentValues.SetValues(good);

      return oldGood;
    }

    // Select

    public Good EnsureGet(int id)
    {
      var result = Table
        .Include(good => good.items)
        .Where(good => good.id == id)
        .SingleOrDefault();

      if (result == null) {
        throw new FailureOperation("货品不存在");
      }

      return result;
    }

    public Good EnsureGet(int whId, int goodId)
    {
      var good = Table
        .Include(g => g.items)
        .SingleOrDefault(g => g.id == goodId && g.warehouse_id == whId);

      if (good == null) {
        throw new FailureOperation("货品不存在");
      }

      return good;
    }

    public void EnsureUnique(Good good)
    {
      if (
        good.number != null && (good.id == 0 ? DbContext.Goods.Any(g =>
          g.number == good.number &&
          g.warehouse_id == good.warehouse_id      
        ) : DbContext.Goods.Any(g =>
          g.id != good.id &&
          g.number == g.number &&
          g.warehouse_id == g.warehouse_id
        ))
      ) {
        throw new FailureOperation("该货品编码已存在");
      }

      if (
        good.id == 0 ? DbContext.Goods.Any(g =>
          g.name == good.name &&
          g.warehouse_id == good.warehouse_id
        ) : DbContext.Goods.Any(g =>
          g.id != good.id &&
          g.name == good.name &&
          g.warehouse_id == good.warehouse_id
        )
      ) {
        throw new FailureOperation("该货品名称已存在");
      }
    }

  }
}
