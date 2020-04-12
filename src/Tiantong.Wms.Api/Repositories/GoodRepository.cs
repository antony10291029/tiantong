using System.Collections.Generic;
using System.Linq;
using Renet.Web;
using Microsoft.EntityFrameworkCore;

namespace Tiantong.Wms.Api
{
  public class GoodRepository : Repository<Good, int>
  {
    public ItemRepository Items { get; }

    private WarehouseRepository _warehouses;

    public GoodRepository(
      DbContext db,
      ItemRepository items,
      WarehouseRepository warehouses
    ) : base(db) {
      _warehouses = warehouses;
      Items = items;
    }

    // Create

    public override Good Add(Good good)
    {
      EnsureUnique(good);
      Items.EnsureUnique(good.items);
      DbContext.Add(good);

      return good;
    }

    // Delete

    public override bool Remove(Good good)
    {
      var itemIds = good.items.Select(item => item.id).ToArray();

      if (
        DbContext.PurchaseOrderItems.Any(oi => oi.good_id == good.id)
      ) {
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
        .Where(g => g.id == good.id)
        .FirstOrDefault();

      Items.Remove(
        oldGood.items.Where(item =>
          !good.items.Any(i => i.id == item.id)
        )
      );

      foreach (var item in good.items) {
        var oldItem = oldGood.items.Where(i => i.id == item.id).FirstOrDefault();

        if (oldItem == null || item.id == 0) {
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
