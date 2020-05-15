using System.Threading.Tasks;
using NUnit.Framework;
using System.Linq;

namespace Tiantong.Iot.Test
{
  [TestFixture]
  public class WatcherHttpClientTest
  {
    [Test]
    public void TestHttpPostScuccess()
    {
      var manager = new IntervalManager();
      var db = new TestDatabaseProvider().Resolve();
      var listener = new TestWatcherHttpListener();
      var client = new HttpPusherClient(db, manager);
      var url = listener.Start();

      client.PostAsync(0, url, "test").GetAwaiter().GetResult();
      client.PostAsync(0, url, "test").GetAwaiter().GetResult();

      listener.Close();
      // listener.Wait();
      client._logger.HandleLog();
      Assert.AreEqual(2, db.HttpPusherLogs.Count());
    }

    [Test]
    public void TestHttpPostFailure()
    {
      var manager = new IntervalManager();
      var db = new TestDatabaseProvider().Resolve();
      var client = new HttpPusherClient(db, manager);
      var url = $"http://localhost:{Port.Free()}/";

      client.Timeout(1);
      try {
        client.PostAsync(0, url, "test").GetAwaiter().GetResult();
      } catch {}
      try {
        client.PostAsync(0, url, "test").GetAwaiter().GetResult();
      } catch {}

      client._logger.HandleLog();
      Assert.AreEqual(2, db.HttpPusherErrors.Count());
    }
  }
}
