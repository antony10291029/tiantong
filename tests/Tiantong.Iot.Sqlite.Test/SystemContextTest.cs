using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Sqlite.Test
{
  public class TestSystemContext: SystemContext
  {
    static SqliteConnection _connection = new("DataSource=system;mode=memory;cache=shared");

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      if (_connection.State == 0) {
        _connection.Open();
      }

      builder.UseSqlite(_connection);
    }
  }

  [TestFixture]
  public class SystemContextTest
  {
    private SystemContext GetDb()
    {
      var db = new TestSystemContext();

      db.Database.EnsureCreated();

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

      db.Add(new HttpPusher {
        id = 0,
        name = "name",
        number = "1",
        url = "localhost",
        header = "",
        body = "{\"id\": 1}",
        field = "value",
      });

      db.SaveChanges();
    }

    [Test]
    public void TestPlc()
    {
      var db = GetDb();

      db.Add(new Plc() {
        model = "test",
        name = "name",
        number = "1",
        host = "localhost",
        port = 102,
        comment = "comment",
        states = new() {
          new() {
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
            state_http_pushers = new() {
              new() {
                pusher = new () {
                  id = 0,
                  name = "name",
                  number = "1",
                  url = "localhost",
                  header = "",
                  body = "{\"id\": 1}",
                  field = "value",
                }
              }
            }
          }
        }
      });

      db.SaveChanges();
    }

    [Test]
    public void TestKeyValue()
    {
      var db = GetDb();

      db.Add(new KeyValue {
        key = "key",
        value = "value"
      });

      db.SaveChanges();
    }
  }
}
