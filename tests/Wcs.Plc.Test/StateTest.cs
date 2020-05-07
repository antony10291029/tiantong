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
      state.IntervalManager = new IntervalManager();
      state.Name = "test";
      state.UseAddress("D100");

      return state;
    }

    [TestCase(true)]
    public void TestStateBool(bool value)
    {
      var state = ResolveState<StateBool>();
      state.Set(value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase(0)]
    [TestCase(10)]
    [TestCase(1000)]
    public void TestStateUInt16(int value)
    {
      var state = ResolveState<StateUInt16>();
      state.Set((ushort) value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase(0)]
    [TestCase(10)]
    [TestCase(1000)]
    public void TestStateInt32(int value)
    {
      var state = ResolveState<StateInt32>();
      state.Set(value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase("hello word")]
    [TestCase("happy hacking")]
    public void TestStateString(string value)
    {
      var state = ResolveState<StateString>();
      state.Set(value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase(new byte[] { 0x00 })]
    [TestCase(new byte[] { 0x00, 0x01, 0x02 })]
    public void TestStateBytes(byte[] value)
    {
      var state = ResolveState<StateBytes>();
      state.Set(value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase(1)]
    public void TestStateHook(int value)
    {
      var getHookData = 0;
      var setHookData = 0;
      var state = ResolveState<StateInt32>();

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
      var state = ResolveState<StateInt32>();

      state.Collect(0);
      state.AddGetHook(_ => state.CollectInterval.Stop());
      state.Set(100);
      state.CollectInterval.Start().WaitAsync().AssertFinishIn();
    }

    [Test]
    public void TestStateHearteatUnheartbeat()
    {
      var state = ResolveState<StateInt32>();
      var manager = state.IntervalManager;

      state.Heartbeat(0);
      state.AddSetHook(value => state.HeartbeatInterval.Stop());
      state.HeartbeatInterval.Start().WaitAsync().AssertFinishIn();
    }

    [Test]
    public void TestWatcher()
    {
      var state = ResolveState<StateInt32>();

      state.Watch(_ => state.CollectInterval.Stop());
      state.Collect(0);
      state.Set(1);
      state.CollectInterval.Start().WaitAsync().AssertFinishIn();
    }
  }
}
