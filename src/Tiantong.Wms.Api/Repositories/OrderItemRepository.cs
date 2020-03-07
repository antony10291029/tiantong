using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class OrderItemRepository : Repository<OrderItem, int>
  {
    public OrderItemRepository(DbContext db) : base(db)
    {

    }
  }
}
