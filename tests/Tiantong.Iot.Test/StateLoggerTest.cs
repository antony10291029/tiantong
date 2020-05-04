using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Test
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
      var logger = new StateLogger(0, manager, db);
      var state = new StateInt32() {
        _driver = new StateTestDriverProvider().Resolve(),
      };

      dbProvider.Migrate();

      state.Id(1).Address("D1").Build().Use(logger);
      state.Set(100);
      state.Get();
      Task.Delay(10).GetAwaiter().GetResult();
      logger.HandleStateLogs();

      var count = db.PlcStateLogs.Where(item => item.operation == "get").Count();
      Assert.AreEqual(1, count);

      count = db.PlcStateLogs.Where(item => item.operation == "set").Count();
      Assert.AreEqual(1, count);
    }
  }
}
