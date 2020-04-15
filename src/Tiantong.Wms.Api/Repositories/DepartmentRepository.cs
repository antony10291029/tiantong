using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class DepartmentRepository : Repository<Department, int>
  {
    private OrderRepository _orders;

    public DepartmentRepository(
      DbContext db,
      OrderRepository orders
    ) : base(db) {
      _orders = orders;
    }

    // Remove

    public bool IsRemovable(Department department)
    {
      return !_orders.HasDepartment(department.id);
    }

    public override bool Remove(Department department)
    {
      if (!IsRemovable(department)) {
        throw new FailureOperation("部门已被使用，无法删除");
      }

      return true;
    }

    // Ensure

    public Department EnsureGet(int id)
    {
      var department = Table.Where(dp => dp.id == id).SingleOrDefault();

      if (department == null) {
        throw new FailureOperation("部门不存在");
      }

      return department;
    }

    public void EnsureExists(int warehouseId, int id)
    {
      if (!Table.Any(item => item.warehouse_id == warehouseId && item.id == id)) {
        throw new FailureOperation("部门不存在");
      }
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
