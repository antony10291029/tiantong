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
      var watcher = new Watcher<int>();
      
      watcher.When(value => value != 0).On(value => result = value);
      watcher.Emit(1);

      Assert.AreEqual(result, 1);
    }

    [Test]
    public void TestCustomEvent()
    {
      var result = 0;
      var watcher = new Watcher<WatcherUser>();
      var user = new WatcherUser() { Id = 1, Name = "test" };

      watcher.When(item => item.Id == 10).On(item => result = item.Id);

      watcher.Emit(user);
      Assert.AreEqual(result, 0);

      user.Id = 10;
      watcher.Emit(user);
      Assert.AreEqual(result, 10);
    }
  }
}
