using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class StockRepository : Repository<Stock>
  {
    private OrderItemRepository _orderItems;

    private StockRecordRepository _stockRecords;

    public StockRepository(
      DbContext db,
      OrderItemRepository orderItems,
      StockRecordRepository stockRecords
    ) : base(db) {
      _orderItems = orderItems;
      _stockRecords = stockRecords;
    }

    public Stock GetOrAdd(int warehouseId, int itemId, int locationId)
    {
      var stock = Table.Where(sk =>
        sk.item_id == itemId &&
        sk.location_id == locationId &&
        sk.warehouse_id == warehouseId
      ).FirstOrDefault();

      if (stock == null) {
        stock = new Stock {
          item_id = itemId,
          location_id = locationId,
          warehouse_id = warehouseId
        };
        Add(stock);
      }

      return stock;
    }
  }
}
