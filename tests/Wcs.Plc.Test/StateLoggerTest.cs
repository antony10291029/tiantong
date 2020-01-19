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
      var container = new PlcContainer();
      var logger = new StateLogger(container);
      var db = container.ResolveDbContext();
      var state = new StateWord(container);
      var connection = new PlcConnection() { Id = 1 };
      var count = 0;

      container.PlcConnection = connection;
      state.Use(logger);

      state.Name = "test";
      state.Key = "D1";
      state.Length = 1;
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
