using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class ProjectRepository : Repository<Project, int>
  {
    private WarehouseRepository _warehouses;

    public ProjectRepository(DbContext db, WarehouseRepository warehouses) : base(db)
    {
      _warehouses = warehouses;
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

    public Project EnsureGet(int id)
    {
      var project = Get(id);

      if (project == null) {
        throw new HttpException("Project id does not exist");
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
        throw new HttpException("Project number already exists in this warehouse");
      }
    }
  }
}
