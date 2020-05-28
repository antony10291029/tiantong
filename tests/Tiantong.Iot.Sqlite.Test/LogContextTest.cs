using NUnit.Framework;
using Tiantong.Iot.Entities;
using Tiantong.Iot.Sqlite.Log;

namespace Tiantong.Iot.Sqlite.Test
{
  [TestFixture]
  public class DbContextTest
  {
    private LogContext GetDb()
    {
      var db = new SqliteLogContext();
      var mg = new SqliteLogMigrator(db);

      db.UseInMemory();
      mg.Migrate();

      return db;
    }

    [Test]
    public void TestMigrator()
    {
      var db = GetDb();

      Assert.IsTrue(db.HasTable("plc_logs"));
    }

    [Test]
    public void TestEmailVerifyCode()
    {
      var db = GetDb();

      db.EmailVerifyCodes.Add(new EmailVerifyCode() {
        email = "test",
        verify_code = "100000"
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
        message = "message",
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
        operation = StateOperation.Read,
        value = "1",
        message = "detail",
      });

      db.SaveChanges();
    }

    [Test]
    public void TestHttpPusherLog()
    {
      var db = GetDb();

      db.HttpPusherLogs.Add(new HttpPusherLog {
        id = 0,
        pusher_id = 0,
        request = "data",
        response = "data",
        status_code = "400",
      });

      db.SaveChanges();
    }

    [Test]
    public void TestHttpPusherError()
    {
      var db = GetDb();

      db.HttpPusherErrors.Add(new HttpPusherError {
        id = 0,
        pusher_id = 0,
        message = "error",
      });

      db.SaveChanges();
    }
  }
}
