using System.Linq;
using NUnit.Framework;
using Wcs.Plc.Entities;

namespace Wcs.Plc.Test
{
  [TestFixture]
  public class StateLoggerTest
  {
    [Test]
    public void TestLogger()
    {
      var manager = new IntervalManager();
      var dbProvider = new TestDatabaseProvider();
      var db = dbProvider.Resolve();
      var plcConnection = new PlcConnection();
      var logger = new StateLogger(manager, db, plcConnection);
      var state = new StateInt() {
        Event = new Event(),
        IntervalManager = new IntervalManager(),
        StateClient = new StateTestClient() { Store = new StateTestClientStore() }
      };
      var connection = new PlcConnection() { Id = 1 };
      var count = 0;

      dbProvider.Migrate();
      plcConnection = connection;

      state.Use(logger);
      state.Name = "test";
      state.UseAddress("D1", 1);
      state.Set(100);
      state.Get();
      logger.HandleStateLogs();

      count = db.PlcStateLogs.Where(item => item.Operation == "read").Count();
      Assert.AreEqual(1, count);

      count = db.PlcStateLogs.Where(item => item.Operation == "write").Count();
      Assert.AreEqual(1, count);
    }
  }
}
