using NUnit.Framework;
using Wcs.Plc.Entities;

namespace Wcs.Plc
{
  [TestFixture]
  public class StateManagerTest
  {
    private StateManager ResolveManager()
    {
      var event_ = new Event();
      var plcConnection = new PlcConnection();
      var db = (new DatabaseProvider()).Resolve();
      var intervalManager = new IntervalManager();
      var driverProvider = new StateTestDriverProvider();
      var logger = new StateLogger(intervalManager, db, plcConnection);
      var manager = new StateManager(event_, intervalManager, driverProvider,logger);

      return manager;
    }

    [Test]
    public void TestManagerStates()
    {
      var manager = ResolveManager();
      var states = manager.States;

      manager.SetName("bool").Bool("D1");
      manager.SetName("int").Int("D3");
      manager.SetName("string").String("D4");

      var bool_ = (IStateBool) states["bool"];
      var int_ = (IStateInt) states["int"];
      var string_ = (IStateString) states["string"];

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
      var states = manager.States;

      manager.SetName("bool").Bool("D1");
      states.Remove("D1");

      try {
        var state = states["D1"];
        Assert.Fail("state should be removed");
      } catch {}
    }
  }
}
