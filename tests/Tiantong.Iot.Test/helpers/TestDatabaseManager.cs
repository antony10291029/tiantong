using Tiantong.Iot.Entities;
using Tiantong.Iot.DB.Sqlite;

namespace Tiantong.Iot.Test
{
  public class TestDatabaseManager : DatabaseManager
  {
    private IotDbContext _db;

    public override IotDbContext Resolve(bool managed = true)
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

    public override void Migrate()
    {
      var db = Resolve();
      new IotSqliteMigrator(db).Migrate();
    }

  }

}
