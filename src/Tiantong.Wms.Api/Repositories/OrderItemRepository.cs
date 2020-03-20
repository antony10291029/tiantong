using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class OrderItemRepository : Repository<OrderItem>
  {
    public OrderItemRepository(DbContext db) : base(db)
    {

    }

    public void EnsureKeys(int[] keys)
    {
      if (keys.Length != keys.Distinct().Count()) {
        throw new HttpException("Order item keys repeat");
      }
    }

    public bool HasItem(int itemId)
    {
      return Table.Any(item => item.item_id == itemId);
    }
  }
}
