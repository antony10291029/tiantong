using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class OrderRepository : Repository<Order, int>
  {
    public OrderRepository(DbContext db) : base(db)
    {

    }
  }
}
