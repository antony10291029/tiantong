using System.Linq;
using NUnit.Framework;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.DB.Sqlite.Test
{
  [TestFixture]
  public class DbContextTest
  {
    private SqliteDbContext GetInitializedDB()
    {
      var db = new SqliteDbContext();
      var migrator = new Migrator();

      db.UseInMemory();
      migrator.UseDbContext(db).Migrate();

      return db;
    }

    [Test]
    public void TestMigrator()
    {
      var db = GetInitializedDB();

      Assert.IsTrue(db.HasTable("plcs"));
    }

  }
}
