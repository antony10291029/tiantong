using System.Collections.Generic;
using Tiantong.Iot.Entities;
using Tiantong.Iot.DB.Sqlite;

namespace Tiantong.Iot
{
  public class DatabaseManager
  {
    private List<IotDbContext> DbPool = new List<IotDbContext>();

    public virtual IotDbContext Resolve(bool managed = true)
    {
      var dbContext = new IotSqliteDbcontext();

      if (managed) {
        DbPool.Add(dbContext);
      }

      return dbContext;
    }

    public virtual void DisposeDbPool()
    {
      foreach (var db in DbPool) {
        db.Dispose();
      }

      DbPool.Clear();
    }

    public virtual void Migrate()
    {
      using (var db = Resolve(false)) {
        new IotSqliteMigrator(db).Migrate();
      }
    }

  }
}
