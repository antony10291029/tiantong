using System.Linq;
using NUnit.Framework;
using Wcs.Plc.Entities;

namespace Wcs.Plc.DB.Sqlite.Test
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

      Assert.IsTrue(db.HasTable("event_logs"));
    }

    [Test]
    public void TestEntity()
    {
      var db = GetInitializedDB();
      var plcConnection = new PlcConnection {
        Model = "melsec-q",
        Name = "lift",
        IsRunning = true,
      };
      var plcConnectionLog = new PlcConnectionLog {
        PlcId = 1,
        Operation = "test",
      };
      var plcStateLog = new PlcStateLog {
        PlcId = 1,
        Operation = "test",
        Name = "test",
        Key = "D1001",
        Length = 1,
        Value = "100"
      };

      db.PlcConnections.Add(plcConnection);
      db.PlcConnectionLogs.Add(plcConnectionLog);
      db.PlcStateLogs.Add(plcStateLog);
      db.SaveChanges();

      Assert.AreEqual(1, db.PlcConnections.Where(item => item.Name == "lift").Count());
      Assert.AreEqual(1, db.PlcConnectionLogs.Where(item => item.Operation == "test").Count());
      Assert.AreEqual(1, db.PlcStateLogs.Where(item => item.Operation == "test").Count());
    }

  }
}
