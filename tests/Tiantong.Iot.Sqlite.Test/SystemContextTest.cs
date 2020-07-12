using NUnit.Framework;
using Tiantong.Iot.Entities;
using Tiantong.Iot.Sqlite.System;

namespace Tiantong.Iot.Sqlite.Test
{
  [TestFixture]
  public class SystemContextTest
  {
    private SystemContext GetDb()
    {
      var db = new SqliteSystemContext();
      var mg = new SqliteSystemMigrator(db);

      db.UseInMemory();
      mg.Migrate();

      return db;
    }

    [Test]
    public void TestMigrator()
    {
      var db = GetDb();

      Assert.IsTrue(db.HasTable("plcs"));
    }

    [Test]
    public void TestHttpPusher()
    {
      var db = GetDb();

      db.HttpPushers.Add(new HttpPusher {
        id = 0,
        name = "name",
        number = "1",
        when_opt = "!=",
        when_value = "value",
        url = "localhost",
        header = "",
        body = "{\"id\": 1}",
        value_key = "value",
        is_concurrent = true,
        is_value_to_string = true,
      });

      db.SaveChanges();
    }

    [Test]
    public void TestPlc()
    {
      var db = GetDb();

      db.Plcs.Add(new Plc() {
        model = "test",
        name = "name",
        number = "1",
        host = "localhost",
        port = 102,
        comment = "comment",
      });

      db.SaveChanges();
    }

    [Test]
    public void TestKeyValue()
    {
      var db = GetDb();

      db.KeyValues.Add(new KeyValue {
        key = "key",
        value = "value"
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
        number = "1",
        type = "uint16",
        address = "d100",
        length = 1,
        is_heartbeat = true,
        heartbeat_interval = 1000,
        heartbeat_max_value = 1000,
        is_collect = true,
        collect_interval = 1000,
        is_read_log_on = true,
        is_write_log_on = true,
      });

      db.SaveChanges();
    }

    [Test]
    public void TestStateHttpPushers()
    {
      var db = GetDb();

      db.PlcStateHttpPushers.Add(new PlcStateHttpPusher {
        id = 0,
        state_id = 0,
        pusher_id = 0,
      });

      db.SaveChanges();
    }

  }

}
