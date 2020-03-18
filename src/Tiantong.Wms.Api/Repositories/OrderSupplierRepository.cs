using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class OrderSupplierRepository : Repository<OrderSupplier>
  {
    public OrderSupplierRepository(DbContext db) : base(db)
    {

    }

    public bool HasSupplier(int supplierId)
    {
      return Table.Any(order => order.supplier_id == supplierId);
    }
  }
}
