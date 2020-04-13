using System.Collections.Generic;
using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class StockRepository : Repository<Stock>
  {
    public StockRepository(DbContext db): base(db)
    {

    }

    public Stock GetOrNew(int warehouseId, int goodId, int itemId, int areaId, int locationId)
    {
      var stock = Get(warehouseId, goodId, itemId, areaId, locationId);

      if (stock == null) {
        stock = new Stock {
          good_id = goodId,
          item_id = itemId,
          area_id = areaId,
          location_id = locationId,
          warehouse_id = warehouseId
        };
      }

      return stock;
    }

    // search

    public Stock Get(int warehouseId, int goodId, int itemId, int areaId, int locationId)
    {
      return Table.SingleOrDefault(s =>
        s.item_id == itemId &&
        s.location_id == locationId &&
        s.warehouse_id == warehouseId
      );
    }

    public bool HasGood(int goodId)
    {
      return Table.Any(stock => stock.good_id == goodId);
    }

    public bool HasItem(int itemId)
    {
      return Table.Any(stock => stock.item_id == itemId);
    }

    public bool HasItem(int[] itemIds)
    {
      return Table.Any(stock => itemIds.Contains(stock.item_id));
    }

  }
}
