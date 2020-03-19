using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class OrderProjectRepository : Repository<OrderProject>
  {
    public OrderProjectRepository(DbContext db) : base(db)
    {

    }

    public bool HasProject(int warehouseId, int projectId)
    {
      return Table.Any(ct => ct.warehouse_id == warehouseId && ct.project_id == projectId);
    }
  }
}
