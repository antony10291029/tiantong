using Microsoft.EntityFrameworkCore;

namespace Yuchuan.IErp.Api
{
  public abstract class Repository
  {
    private DbContext _db;

    public Repository(DbContext db)
    {
      _db = db;
    }

    public int SaveChanges() => _db.SaveChanges();
  }
}
