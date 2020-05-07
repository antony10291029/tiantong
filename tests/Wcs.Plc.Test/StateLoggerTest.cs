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
      var state = new StateInt32() {
        IntervalManager = new IntervalManager(),
        Driver = new StateTestDriver() { Store = new StateTestDriverStore() }
      };
      var connection = new PlcConnection() { Id = 1 };
      var count = 0;

      dbProvider.Migrate();
      plcConnection = connection;

      state.Use(logger);
      state.Name = "test";
      state.UseAddress("D1");
      state.Set(100);
      state.Get();
      logger.HandleStateLogs();

      count = db.PlcStateLogs.Where(item => item.operation == PlcStateOperation.Read).Count();
      Assert.AreEqual(1, count);

      count = db.PlcStateLogs.Where(item => item.operation == PlcStateOperation.Write).Count();
      Assert.AreEqual(1, count);
    }
  }
}
