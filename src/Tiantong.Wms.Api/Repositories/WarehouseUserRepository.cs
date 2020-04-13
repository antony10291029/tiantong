using System.Linq;
using Renet.Web;
using Microsoft.EntityFrameworkCore;

namespace Tiantong.Wms.Api
{
  public class WarehouseUserRepository : Repository<WarehouseUser, int>
  {
    private UserRepository _users;

    public WarehouseUserRepository(
      DbContext db,
      UserRepository users
    ) : base(db) {
      _users = users;
    }

    public override WarehouseUser Add(WarehouseUser wu)
    {
      var user = _users.Table
        .Where(item => item.email == wu.user.email)
        .FirstOrDefault();

      if (user != null) {
        wu.user_id = wu.user.id = user.id;

        EnsureUnique(wu);
        _users.Update(user, wu.user);

        wu.user = null;
      } else {
        wu.user.type = "keeper";
        wu.user.password = "123456";
        _users.EnsureUnique(wu.user);
        _users.EncodePassword(wu.user);
      }

      DbContext.Add(wu);

      return wu;
    }

    public WarehouseUser Get(int warehouseId, int userId)
    {
      return Table
        .Include(wu => wu.user)
        .Where(wu => wu.warehouse_id == warehouseId && wu.user_id == userId)
        .FirstOrDefault();
    }

    public void EnsureUnique(WarehouseUser wu)
    {
      if (
        Table.Any(w =>
          w.warehouse_id == wu.warehouse_id &&
          w.user_id == wu.user_id
        )
      ) {
        throw new FailureOperation("该用户已存在");
      }
    }

    public WarehouseUser EnsureGet(int id)
    {
      var wu = Table.Where(item => item.id == id).SingleOrDefault();

      if (wu == null) {
        throw new FailureOperation("用户不存在");
      }

      return wu;
    }

  }
}
