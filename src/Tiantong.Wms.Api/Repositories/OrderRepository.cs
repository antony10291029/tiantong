using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class OrderRepository : Repository<Order, int>
  {
    public OrderRepository(DbContext db) : base(db)
    {

    }

    public bool HasNumber(int warehouseId, string number)
    {
      return Table.Any(order => order.warehouse_id == warehouseId && order.number == number);
    }

    public void EnsureNumberUnique(int warehouseId, string number)
    {
      if (HasNumber(warehouseId, number)) {
        throw new HttpException("Order number already exists in this warehouse");
      }
    }

  }
}
