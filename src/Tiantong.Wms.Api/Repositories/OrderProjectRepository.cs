using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class OrderProjectRepository : Repository<OrderProject>
  {
    public OrderProjectRepository(DbContext db) : base(db)
    {

    }
  }
}
