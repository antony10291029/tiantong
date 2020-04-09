using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class DepartmentRepository : Repository<Department, int>
  {
    public DepartmentRepository(DbContext db) : base(db)
    {

    }

    public void Ensure(int warehouseId, int id)
    {
      if (!Table.Any(item => item.warehouse_id == warehouseId && item.id == id)) {
        throw new FailureOperation("部门不存在");
      }
    }

    public Department EnsureGet(int id)
    {
      var department = Table.Where(dp => dp.id == id).SingleOrDefault();

      if (department == null) {
        throw new FailureOperation("部门不存在");
      }

      return department;
    }

    public void EnsureUnique(Department department)
    {
      if (
        Table.Any(item =>
          item.id != department.id &&
          item.name == department.name &&
          item.warehouse_id == department.warehouse_id
        )
      ) {
        throw new FailureOperation("部门名称重复");
      }
    }

  }
}
