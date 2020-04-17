using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class ProjectRepository : Repository<Project, int>
  {
    private BaseOrderRepository _orders;

    public ProjectRepository(
      DbContext db,
      BaseOrderRepository orders
    ) : base(db) {
      _orders = orders;
    }

    //

    public Project[] Search(int warehouseId)
    {
      return Table.Where(project => project.warehouse_id == warehouseId)
        .OrderBy(project => project.number)
        .ToArray();
    }

    // Remove

    public bool IsRemovable(Project project)
    {
      return !_orders.HasProject(project.id);
    }

    public override bool Remove(Project project)
    {
      if (!IsRemovable(project)) {
        throw new FailureOperation("工程已被使用，不可删除");
      }

      DbContext.Remove(project);

      return true;
    }

    public bool HasNumber(int warehouseId, string number)
    {
      return Table.Any(project => project.warehouse_id == warehouseId && project.number == number);
    }

    public bool HasId(int warehouseId, int id)
    {
      return Table.Any(project => project.warehouse_id == warehouseId && project.id == id);
    }

    public void EnsureExists(int warehouseId, int[] projectIds)
    {
      if (
        !Table.Any(p =>
          projectIds.Contains(p.id) &&
          p.warehouse_id == warehouseId
        )
      ) {
        throw new FailureOperation("工程不存在");
      }
    }

    public Project EnsureGet(int id)
    {
      var project = Get(id);

      if (project == null) {
        throw new FailureOperation("工程不存在");
      }

      return project;
    }

    public void EnsureId(int warehouseId, int id)
    {
      if (!HasId(warehouseId, id)) {
        throw new FailureOperation("工程不存在");
      }
    }

    public void EnsureNumberUnique(int warehouseId, string number)
    {
      if (HasNumber(warehouseId, number)) {
        throw new HttpException("工程代码已存在");
      }
    }
  }
}
