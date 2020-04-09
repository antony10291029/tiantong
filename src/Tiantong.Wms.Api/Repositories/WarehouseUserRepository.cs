using System.Linq;
using Renet.Web;

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

    public WarehouseUser Get(int warehouseId, int userId)
    {
      return Table.Where(
        wu => wu.warehouse_id == warehouseId && wu.user_id == userId
      ).FirstOrDefault();
    }

    public WarehouseUser EnsureGet(int id)
    {
      var wu = Table.Where(item => item.id == id).SingleOrDefault();

      if (wu == null) {
        throw new FailureOperation("用户不存在");
      }

      return wu;
    }

    public void EnsureUnique(WarehouseUser user)
    {
      if (user != null) {
        _users.EnsureUnique(user.user);
      } else if (
        Table.Any(wu =>
          wu.user_id == user.id &&
          wu.warehouse_id == user.warehouse_id
        )
      ) {
        throw new FailureOperation("仓库中已有该用户");
      }
    }

  }
}
