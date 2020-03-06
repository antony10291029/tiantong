using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class StockRepository : Repository<Stock, int>
  {
    public StockRepository(DbContext db) : base(db)
    {

    }
  }
}
