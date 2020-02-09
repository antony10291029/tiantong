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
      var db = (new TestDatabaseProvider()).Resolve();
      var manager = new IntervalManager();
      var logger = new EventLogger(manager, db);
      var event_ = new Event();

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
