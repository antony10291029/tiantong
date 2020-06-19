using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class DepartmentRepository : Repository<Department, int>
  {
    private BaseOrderRepository _orders;

    private WarehouseRepository _warehouses;

    public DepartmentRepository(
      DbContext db,
      BaseOrderRepository orders,
      WarehouseRepository warehouses
    ) : base(db) {
      _orders = orders;
      _warehouses = warehouses;
    }

    // Create

    public override Department Add(Department department)
    {
      _warehouses.Users.EnsureOwner(department.warehouse_id);
      EnsureUnique(department);
      department.type = DepartmentType.Custom;
      DbContext.Add(department);

      return department;
    }

    // Remove

    public bool IsRemovable(Department department)
    {
      return !_orders.HasDepartment(department.id);
    }

    public override bool Remove(int id)
    {
      var department = EnsureGet(id);
      _warehouses.Users.EnsureOwner(department.warehouse_id);
      if (department.type != DepartmentType.Custom) {
        throw new FailureOperation("系统部门不可删除");
      }
      if (!IsRemovable(department)) {
        throw new FailureOperation("该部门已被使用，不可删除");
      }
      DbContext.Remove(department);

      return true;
    }

    // Update

    public override Department Update(Department department)
    {
      var oldDepartment = EnsureGet(department.id);
      if (oldDepartment.type != DepartmentType.Custom) {
        throw new FailureOperation("系统默认部门不可修改");
      }
      _warehouses.Users.EnsureOwner(department.warehouse_id);

      DbContext.Entry(oldDepartment).CurrentValues.SetValues(department);
      DbContext.Entry(oldDepartment).Property(dp => dp.type).IsModified = false;

      return oldDepartment;
    }

    // Ensure

    public Department Find(int id)
    {
      var department = EnsureGet(id);

      _warehouses.Users.EnsureUser(department.warehouse_id);

      return department;
    }

    public IEntities<Department, int> All(int warehouseId, string search, string type)
    {
      _warehouses.Users.EnsureUser(warehouseId);

      return Table
        .Where(department =>
          (department.warehouse_id == warehouseId) &&
          (type == null ? true : department.type == type) &&
          (search == null ? true : department.name.Contains(search))
        )
        .ToEntities();
    }

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
