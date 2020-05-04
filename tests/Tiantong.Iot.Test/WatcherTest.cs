using System.Text.Json;
using NUnit.Framework;

namespace Tiantong.Iot.Test
{
  [TestFixture]
  public class WatcherTest
  {
    [Test]
    public void TestHandleCancel()
    {
      var result = 0;
      var watcher = new Watcher<int>(new TestWatcherHttpClient());
      
      watcher.When(value => value != 0).On(value => result = value);
      watcher.Emit(1);

      Assert.AreEqual(result, 1);
    }

    [Test]
    public void TestCustomEvent()
    {
      var result = 0;
      var watcher = new Watcher<WatcherUser>(new TestWatcherHttpClient());
      var user = new WatcherUser() { Id = 1, Name = "test" };

      watcher.When(item => item.Id == 10).On(item => result = item.Id);

      watcher.Emit(user);
      Assert.AreEqual(result, 0);

      user.Id = 10;
      watcher.Emit(user);
      Assert.AreEqual(result, 10);
    }

    [Test]
    public void TestHttpPostNumber()
    {
      var client = new TestWatcherHttpClient();
      var watcher = new Watcher<int>(client);
      watcher.When(_ => true).HttpPost("test", "key", false, "{\"field\": 1}");
      watcher.Emit(100);

      Assert.AreEqual(client.Url, "test");
      Assert.AreEqual(JsonDocument.Parse(client.Data).RootElement.GetProperty("key").GetInt32(), 100);
      Assert.AreEqual(JsonDocument.Parse(client.Data).RootElement.GetProperty("field").GetInt32(), 1);
    }

    [Test]
    public void TestHttpPostNumberToString()
    {
      var client = new TestWatcherHttpClient();
      var watcher = new Watcher<int>(client);
      watcher.When(_ => true).HttpPost("test", "key", true);
      watcher.Emit(100);

      Assert.AreEqual(client.Url, "test");
      Assert.AreEqual(JsonDocument.Parse(client.Data).RootElement.GetProperty("key").GetString(), "100");
    }

    [Test]
    public void TestHttpPostString()
    {
      var client = new TestWatcherHttpClient();
      var watcher = new Watcher<string>(client);
      watcher.When(_ => true).HttpPost("test", "string");
      watcher.Emit("hello world");

      Assert.AreEqual(client.Url, "test");
      Assert.AreEqual(JsonDocument.Parse(client.Data).RootElement.GetProperty("string").GetString(), "hello world");
    }

    [Test]
    public void TestHttpPostStringToString()
    {
      var client = new TestWatcherHttpClient();
      var watcher = new Watcher<string>(client);
      watcher.When(_ => true).HttpPost("test", "string", true);
      watcher.Emit("hello world");

      Assert.AreEqual(client.Url, "test");
      Assert.AreEqual(JsonDocument.Parse(client.Data).RootElement.GetProperty("string").GetString(), "hello world");
    }

  }
}
