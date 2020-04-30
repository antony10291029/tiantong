using System.Threading.Tasks;
using System.Reflection;
using NUnit.Framework;

namespace Wcs.Plc.Test
{
  [TestFixture]
  public class StateTest
  {
    public T ResolveState<T>() where T : State, new()
    {
      var state = new T();
      var driver = new StateTestDriver() {
        Store = new StateTestDriverStore()
      };

      state.Driver = driver;
      state.Event = new Event();
      state.IntervalManager = new IntervalManager();
      state.Name = "test";
      state.UseAddress("D100", 1);

      return state;
    }

    [TestCase(0)]
    public void TestStateInt(int value)
    {
      var state = ResolveState<StateInt>();
      state.Set(value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase("happy hacking")]
    public void TestStateString(string value)
    {
      var state = ResolveState<StateString>();
      state.Set(value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase(true)]
    public void TestStateBool(bool value)
    {
      var state = ResolveState<StateBool>();
      state.Set(value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase(1)]
    public void TestStateHook(int value)
    {
      var getHookData = 0;
      var setHookData = 0;
      var state = ResolveState<StateInt>();

      state.AddSetHook(data => setHookData = data);
      state.Set(value);
      Task.Delay(3).GetAwaiter().GetResult();
      Assert.AreEqual(value, setHookData);

      state.AddGetHook(data => getHookData = data);
      state.Get();
      Task.Delay(3).GetAwaiter().GetResult();
      Assert.AreEqual(value, getHookData);
    }

    [Test]
    public void TestStateCollectAndUncollect()
    {
      var state = ResolveState<StateInt>();

      state.Collect(0);
      state.AddGetHook(_ => state.UncollectAsync());
      state.Set(100);
      state.IntervalManager.Start().WaitAsync().AssertFinishIn();
    }

    [Test]
    public void TestStateHearteatUnheartbeat()
    {
      var state = ResolveState<StateInt>();
      var manager = state.IntervalManager;

      state.Heartbeat(0);
      state.AddSetHook(value => state.UnheartbeatAsync());
      manager.Start().WaitAsync().AssertFinishIn();
    }

    [Test]
    public void TestWatcher()
    {
      var state = ResolveState<StateInt>();
      var event_ = new Event();

      state.Event = event_;
      event_.On<int>("watch", _ => state.Uncollect());
      state.Watch().Event("watch");
      state.Collect(0);
      state.Set(1);
      state.IntervalManager.Start().WaitAsync().AssertFinishIn();
    }
  }
}
