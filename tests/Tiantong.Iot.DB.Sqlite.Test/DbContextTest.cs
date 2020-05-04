using System.Linq;
using NUnit.Framework;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.DB.Sqlite.Test
{
  [TestFixture]
  public class DbContextTest
  {
    private SqliteDbContext GetDb()
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
      var db = GetDb();

      Assert.IsTrue(db.HasTable("plcs"));
    }

    [Test]
    public void TestPlc()
    {
      var db = GetDb();

      db.Plcs.Add(new Plc() {
        model = "test",
        name = "name",
        host = "localhost",
        port = 102,
        comment = "comment",
      });

      db.SaveChanges();
    }

    [Test]
    public void TestPlcLog()
    {
      var db = GetDb();

      db.PlcLogs.Add(new PlcLog {
        id = 0,
        plc_id = 0,
        type = "warning",
        operation = "run",
        detail = "detail",
      });

      db.SaveChanges();
    }

    [Test]
    public void TestPlcError()
    {
      var db = GetDb();

      db.PlcErrors.Add(new PlcError {
        id = 0,
        plc_id = 0,
        error = "timeout",
        detail = "timeout"
      });

      db.SaveChanges();
    }

    [Test]
    public void TestPlcState()
    {
      var db = GetDb();

      db.PlcStates.Add(new PlcState {
        id = 0,
        plc_id = 0,
        name = "name",
        type = "uint16",
        address = "d100",
        length = 1,
        is_heartbeat = true,
        heartbeat_interval = 1000,
        heartbeat_max_value = 1000,
        is_collect = true,
        collect_interval = 1000,
        open_log = true
      });

      db.SaveChanges();
    }

    [Test]
    public void TestStateLog()
    {
      var db = GetDb();

      db.PlcStateLogs.Add(new PlcStateLog {
        id = 0,
        plc_id = 0,
        state_id = 0,
        operation = "read",
        value = "1000",
      });

      db.SaveChanges();
    }

    [Test]
    public void TestStateError()
    {
      var db = GetDb();

      db.PlcStateErrors.Add(new PlcStateError {
        id = 0,
        plc_id = 0,
        state_id = 0,
        error = "error",
        detail = "detail",
      });

      db.SaveChanges();
    }

    [Test]
    public void TestHttpWatcher()
    {
      var db = GetDb();

      db.HttpWatchers.Add(new HttpWatcher {
        id = 0,
        plc_id = 0,
        state_id = 0,
        url = "localhost",
        data = "{\"id\": 1}",
        value_key = "value",
        to_string = true,
      });

      db.SaveChanges();
    }

    [Test]
    public void TestHttpWatcherLog()
    {
      var db = GetDb();

      db.HttpWatcherLogs.Add(new HttpWatcherLog {
        id = 0,
        plc_id = 0,
        state_id = 0,
        watcher_id = 0,
        request = "data",
        response = "data",
        status_code = "400",
      });

      db.SaveChanges();
    }
  }
}
