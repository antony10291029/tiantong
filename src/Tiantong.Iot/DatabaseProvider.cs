using System.IO;
using Tiantong.Iot.Database;
using Tiantong.Iot.DB.Sqlite;

namespace Tiantong.Iot
{
  public class DatabaseProvider
  {
    public virtual DbContext Resolve()
    {
      var db = new SqliteDbContext();

      if (!Directory.Exists("./DataSource")) {
        Directory.CreateDirectory("./DataSource");
      }
      db.UseFile("./DataSource/sqlite.db");

      return db;
    }

    public virtual void Migrate()
    {
      var db = Resolve();
      var migrator = new Migrator();

      migrator.UseDbContext(db).Migrate();
    }
  }
}
