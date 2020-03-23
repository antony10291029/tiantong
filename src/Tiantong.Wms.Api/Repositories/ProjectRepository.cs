using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class ProjectRepository : Repository<Project, int>
  {
    private WarehouseRepository _warehouses;

    private ProjectItemRepository _projectItems;

    public ProjectRepository(
      DbContext db,
      WarehouseRepository warehouses,
      ProjectItemRepository projectItems
    ) : base(db) {
      _warehouses = warehouses;
      _projectItems = projectItems;
    }

    //

    public Project[] Search(int warehouseId)
    {
      return Table.Where(project => project.warehouse_id == warehouseId)
        .OrderBy(project => project.number)
        .ToArray();
    }

    public bool HasNumber(int warehouseId, string number)
    {
      return Table.Any(project => project.warehouse_id == warehouseId && project.number == number);
    }

    public bool HasId(int warehouseId, int id)
    {
      return Table.Any(project => project.warehouse_id == warehouseId && project.id == id);
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

    public Project EnsureGet(int id, int warehouseId, int userId)
    {
      var project = Get(id);

      if (
        project == null ||
        project.warehouse_id != warehouseId ||
        !_warehouses.HasOwner(warehouseId, userId)
      ) {
        throw new FailureOperation("工程不存在");
      }

      return project;
    }

    public Project EnsureGetByOwner(int id, int userId)
    {
      var project = EnsureGet(id);
      _warehouses.EnsureOwner(project.warehouse_id, userId);

      return project;
    }

    public void EnsureNumberUnique(int warehouseId, string number)
    {
      if (HasNumber(warehouseId, number)) {
        throw new HttpException("工程代码已存在");
      }
    }
  }
}
