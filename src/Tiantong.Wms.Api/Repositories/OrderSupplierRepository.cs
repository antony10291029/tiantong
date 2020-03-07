using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class OrderSupplierRepository : Repository<OrderSupplier>
  {
    public OrderSupplierRepository(DbContext db) : base(db)
    {

    }
  }
}
