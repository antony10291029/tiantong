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
      var dbManager = new TestDatabaseManager();
      var db = dbManager.Resolve();
      var logger = new StateLogger(0, manager);
      var state = new StateInt32() {
        _driver = new StateTestDriverProvider().Resolve(),
      };

      dbManager.Migrate();
      logger.UseDbContext(db);

      state.IsReadLogOn(true).IsWriteLogOn(true);
      state.Id(1).Address("D1").Use(logger).Build();
      state.Set(100);
      state.Get();
      Task.Delay(10).GetAwaiter().GetResult();
      logger.HandleLog();

      var count = db.PlcStateLogs.Where(item => item.operation == StateOperation.Read).Count();
      Assert.AreEqual(1, count);

      count = db.PlcStateLogs.Where(item => item.operation == StateOperation.Write).Count();
      Assert.AreEqual(1, count);
    }
  }
}
