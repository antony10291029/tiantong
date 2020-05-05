using NUnit.Framework;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Test
{
  [TestFixture]
  public class StateManagerTest
  {
    private StateManager ResolveManager()
    {
      var db = (new DatabaseProvider()).Resolve();
      var intervalManager = new IntervalManager();
      var driverProvider = new StateTestDriverProvider();
      var watcherProvider = new TestWatcherProvider();
      var logger = new StateLogger(0, intervalManager, db);
      var manager = new StateManager(intervalManager, driverProvider,logger, watcherProvider);

      return manager;
    }

    [Test]
    public void TestManagerStates()
    {
      var manager = ResolveManager();
      var states = manager.StatesByName;

      manager.Name("bool").Bool("D1", _ => {});
      manager.Name("int").Int("D3", _ => {});
      manager.Name("string").String("D4", 10, _ => {});

      var bool_ = (IState<bool>) states["bool"];
      var int_ = (IState<int>) states["int"];
      var string_ = (IState<string>) states["string"];

      bool_.Set(true);
      int_.Set(100);
      string_.Set("AAAA");

      Assert.AreEqual(bool_.Get(), true);
      Assert.AreEqual(int_.Get(), 100);
      Assert.AreEqual(string_.Get(), "AAAA");
    }

    [Test]
    public void TestManagerRemove()
    {
      var manager = ResolveManager();
      var states = manager.StatesByName;

      manager.Name("bool").Bool("D1", _ => {});
      states.Remove("D1");

      try {
        var state = states["D1"];
        Assert.Fail("state should be removed");
      } catch {}
    }

  }
}
