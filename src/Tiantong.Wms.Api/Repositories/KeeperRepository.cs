using Microsoft.EntityFrameworkCore;

namespace Tiantong.Wms.Api
{
  public class KeeperRepository : Repository<Keeper>
  {
    protected DbSet<Keeper> Keepers { get => DbContext.Keepers; }

    public KeeperRepository(DbContext db) : base(db)
    {

    }
  }
}
