using System.Net;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class WarehouseRepository : Repository<Warehouse, int>
  {
    private Auth _auth { get; }

    public AreaRepository Areas { get; }

    public LocationRepository Locations { get; }

    public WarehouseUserRepository Users { get; }

    public DepartmentRepository Departments { get; }

    public WarehouseRepository(
      Auth auth,
      DbContext db,
      UserRepository users,
      BaseOrderRepository orders
    ) : base(db) {
      _auth = auth;
      Areas = new AreaRepository(db, this);
      Locations = new LocationRepository(db, this);
      Departments = new DepartmentRepository(db, orders);
      Users = new WarehouseUserRepository(auth, db, users, this);
    }

    public Warehouse Add(Warehouse warehouse, int id)
    {
      var area = new Area { name = "默认区域" };
      var location = new Location { name  = "默认位置" };
      var ownerDepartment = new Department {
        name = "仓库所有者",
        type = DepartmentType.Owner,
      };
      var adminDepartment = new Department {
        name = "仓库管理员",
        type = DepartmentType.Admin,
      };
      var userDepartment = new Department {
        name = "仓库普通成员",
        type = DepartmentType.User,
      };
      var WarehouseUser = new WarehouseUser {
        user_id = id == 0 ? _auth.User.id : id
      };

      UnitOfWork.BeginTransaction();
      DbContext.Add(warehouse);
      UnitOfWork.SaveChanges();

      area.warehouse_id = warehouse.id;
      location.warehouse_id = warehouse.id;
      ownerDepartment.warehouse_id = warehouse.id;
      adminDepartment.warehouse_id = warehouse.id;
      userDepartment.warehouse_id = warehouse.id;
      WarehouseUser.warehouse_id = warehouse.id;

      DbContext.Add(area);
      DbContext.Add(ownerDepartment);
      DbContext.Add(adminDepartment);
      DbContext.Add(userDepartment);

      UnitOfWork.SaveChanges();

      location.area_id = area.id;
      WarehouseUser.department_id = ownerDepartment.id;

      DbContext.Add(location);
      DbContext.Add(WarehouseUser);

      UnitOfWork.SaveChanges();
      UnitOfWork.Commit();

      return warehouse;
    }

    public override Warehouse Add(Warehouse warehouse)
    {
      return Add(warehouse, 0);
    }

    public Warehouse[] Search(int userId)
    {
      return Users.Table
        .Include(wu => wu.warehouse)
        .Where(wu => wu.user_id == userId)
        .Select(wu => wu.warehouse)
        .OrderBy(wh => wh.number)
        .OrderBy(wh => wh.id)
        .ToArray();
    }

    public Warehouse Find(int warehouseId, int userId)
    {
      var warehouse = Users.Table
        .Include(wu => wu.warehouse)
        .Where(wu =>
          wu.user_id == userId &&
          wu.warehouse_id == warehouseId
        )
        .Select(wu => wu.warehouse)
        .First();

      if (warehouse == null) {
        throw new FailureOperation("仓库不存在");
      }

      return warehouse;
    }

    public Warehouse EnsureGet(int id)
    {
      var warehouse = Get(id);

      if (warehouse == null) {
        throw new FailureOperation("仓库不存在");
      }

      return warehouse;
    }

    public Warehouse EnsureGetByOwner(int warehouseId, int userId)
    {
      var warehouse = Users.Table
        .Include(wu => wu.warehouse)
        .FirstOrDefault(wu =>
          wu.user_id == userId &&
          wu.warehouse_id == warehouseId
        )?.warehouse;

      if (warehouse == null) {
        throw new FailureOperation("仓库认证失败");
      }

      return warehouse;
    }

    public void EnsureUser(int warehouseId, int userId)
    {
      if (!Users.IsOwner(warehouseId, userId)) {
        throw new FailureOperation("仓库认证失败");
      }
    }

  }
}
