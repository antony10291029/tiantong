using System.Linq;
using NUnit.Framework;

namespace Wcs.Plc.Test
{
  [TestFixture]
  public class EventLoggerTest
  {
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(10)]
    public void TestLogger(int n)
    {
      var container = new PlcContainer();
      var logger = new EventLogger(container);
      var event_ = new Event();
      var db = container.ResolveDbContext();

      event_.Use(logger);
      logger.Interval.SetTime(0);

      event_.On<int>("test", value => {});

      for (var i = 0; i < n; i++) {
        event_.Emit("test", 100);
      }

      logger.HandleEventLogs();

      var count = db.EventLogs.Where(item => item.Key == "test").Count();

      Assert.AreEqual(n, count);
    }
  }
}
