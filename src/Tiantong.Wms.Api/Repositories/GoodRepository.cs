using System.Collections.Generic;
using System.Linq;
using Renet.Web;
using Microsoft.EntityFrameworkCore;

namespace Tiantong.Wms.Api
{
  public class GoodRepository : Repository<Good, int>
  {
    private WarehouseRepository _warehouses;

    public GoodRepository(DbContext db, WarehouseRepository warehouses) : base(db)
    {
      _warehouses = warehouses;
    }

    //

    public Good[] Search(int warehouseId)
    {
      return Table.Where(good => good.warehouse_id == warehouseId)
        .OrderBy(good => good.number)
        .ToArray();
    }

    public bool HasNumber(int warehouseId, string number)
    {
      return Table.Any(good => good.warehouse_id == warehouseId && good.number == number);
    }

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

    public Good EnsureGetByOwner(int goodId, int userId)
    {
      var good = EnsureGet(goodId);
      _warehouses.EnsureOwner(good.warehouse_id, userId);

      return good;
    }

    public void EnsureIds(int warehouseId, int[] ids)
    {
      if (Table.Where(good => ids.Contains(good.id)).Count() != ids.Length) {
        throw new FailureOperation("货品不存在");
      }
    }

    public void EnsureNumberUnique(int warehouseId, string number)
    {
      if (HasNumber(warehouseId, number)) {
        throw new FailureOperation("货码重复");
      }
    }

  }
}
