using System.Linq;

namespace Tiantong.Wms.Api
{
  public class ProjectItemRepository : Repository<ProjectItem, int>
  {
    public ProjectItemRepository(DbContext db) : base(db)
    {

    }

    public bool HasProject(int warehouseId, int projectId)
    {
      return Table.Any(ct => ct.warehouse_id == warehouseId && ct.project_id == projectId);
    }
  }
}
