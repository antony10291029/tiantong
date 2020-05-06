using Tiantong.Iot.Entities;
using Tiantong.Iot.DB.Sqlite;

namespace Tiantong.Iot.Test
{
  public class TestDatabaseProvider : DatabaseProvider
  {
    private IotDbContext _db;

    public override IotDbContext Resolve()
    {
      if (_db == null) {
        var db = new IotSqliteDbcontext();
        var mg = new IotSqliteMigrator(db);

        db.UseInMemory();
        mg.Migrate();

        _db = db;
      }

      return _db;
    }
  }
}
