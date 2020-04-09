using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class WarehouseUserController : BaseController
  {
    private IAuth _auth;

    private UserRepository _users;

    private WarehouseRepository _warehouses;

    private WarehouseUserRepository _warehouseUsers;

    public WarehouseUserController(
      IAuth auth,
      UserRepository users,
      WarehouseRepository warehouses,
      DepartmentRepository departments,
      WarehouseUserRepository warehouseUsers
    ) {
      _auth = auth;
      _users = users;
      _warehouses = warehouses;
      _warehouseUsers = warehouseUsers;
    }

    public class CreateParams: WarehouseUser
    {
      [Required]
      public override User user { get; set; }
    }

    public object Create([FromBody] WarehouseUser wu)
    {
      _auth.EnsureOwner();
      _warehouses.EnsureOwner(wu.warehouse_id, _auth.User.id);

      var user = _users.Table
        .Where(item => item.email == wu.user.email)
        .FirstOrDefault();

      if (user != null) {
        if (
          _warehouseUsers.Table.Any(item =>
            item.warehouse_id == wu.warehouse_id &&
            item.user_id == user.id
          )
        ) {
          return FailureOperation("用户邮箱已存在");
        }
        wu.user = null;
        wu.user_id = user.id;
      } else {
        wu.user.type = "keeper";
        wu.user.password = "123456";
      }

      _warehouseUsers.Add(wu);
      _warehouseUsers.UnitOfWork.SaveChanges();

      return SuccessOperation("用户已添加", wu.id);
    }

    public class DeleteParams
    {
      public int id { get; set; }
    }

    public object Delete([FromBody] DeleteParams param)
    {
      var wu = _warehouseUsers.EnsureGet(param.id);

      _warehouses.EnsureOwner(wu.warehouse_id, _auth.User.id);

      _warehouseUsers.Remove(wu);
      _warehouseUsers.UnitOfWork.SaveChanges();

      return SuccessOperation("用户已删除");
    }

    public object Update([FromBody] WarehouseUser wu)
    {
      _warehouses.EnsureOwner(wu.warehouse_id, _auth.User.id);
      _warehouseUsers.Update(wu);
      _warehouseUsers.DbContext.Entry(wu.user)
        .Property(o => o.password).IsModified = false;
      _warehouseUsers.UnitOfWork.SaveChanges();

      return SuccessOperation("用户已更新");
    }

    public class FindParams
    {
      public int id { get; set; }
    }

    public WarehouseUser Find([FromBody] FindParams param)
    {
      var wu = _warehouseUsers.EnsureGet(param.id);
      _warehouses.EnsureOwner(wu.warehouse_id, _auth.User.id);
      wu.user = _users.Get(wu.user_id);

      return wu;
    }

    public class AllParams
    {
      public int warehouse_id { get; set; }

      public string search { get; set; } = "";
    }

    public IEntities<User, int> All([FromBody] AllParams param)
    {
      return _warehouseUsers.Table
        .Include(wu => wu.user)
        .OrderBy(wu => wu.id)
        .Where(wu =>
          wu.warehouse_id == param.warehouse_id &&
          (
            param.search == null ? true :
            wu.user.name.Contains(param.search) ||
            wu.user.email.Contains(param.search)
          )
        )
        .Select(wu => wu.user)
        .OrderBy(user => user.name)
        .ToEntities();
    }
  }
}
