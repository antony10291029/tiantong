using System.Threading.Tasks;
using NUnit.Framework;
using System.Linq;

namespace Tiantong.Iot.Test
{
  [TestFixture]
  public class WatcherHttpClientTest
  {
    [Test]
    public void TestHttpPost()
    {
      var manager = new IntervalManager();
      var db = new TestDatabaseProvider().Resolve();
      var listener = new TestWatcherHttpListener();
      var client = new WatcherHttpClient(db, manager);
      var url = listener.Start();

      client.PostAsync(0, 0, 0, url, "test").GetAwaiter().GetResult();
      try {
        client.PostAsync(0, 0, 0, url, "error").GetAwaiter().GetResult();
      } catch {}
      client.HandleLogs();
      Assert.AreEqual(1, db.HttpWatcherLogs.Count());
      Assert.AreEqual(1, db.HttpWatcherErrors.Count());
    }
  }
}
