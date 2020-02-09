using Wcs.Plc.Database;
using Wcs.Plc.DB.Sqlite;

namespace Wcs.Plc.Test
{
  public class TestDatabaseProvider : DatabaseProvider
  {
    private DbContext _db;

    public override DbContext Resolve()
    {
      if (_db == null) {
        var db = new SqliteDbContext();
        var mg = new Migrator();

        db.UseInMemory();
        mg.UseDbContext(db).Migrate();
        _db = db;
      }

      return _db;
    }
  }
}
