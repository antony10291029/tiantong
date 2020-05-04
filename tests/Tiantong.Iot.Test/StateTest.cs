using System.Threading.Tasks;
using System.Reflection;
using NUnit.Framework;

namespace Tiantong.Iot.Test
{
  [TestFixture]
  public class StateTest
  {
    public T ResolveState<T, U>() where T : State<U>, new()
    {
      var state = new T() {
        _intervalManager = new IntervalManager(),
        _driver = new StateTestDriverProvider().Resolve(),
        _watcherProvider = new TestWatcherProvider(),
      };

      state.Name("test").Address("D100").Build();

      return state;
    }

    [TestCase(true)]
    public void TestStateBool(bool value)
    {
      var state = ResolveState<StateBool, bool>();
      state.Set(value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase(0)]
    [TestCase(10)]
    [TestCase(1000)]
    public void TestStateUInt16(int value)
    {
      var state = ResolveState<StateUInt16, ushort>();
      state.Set((ushort) value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase(0)]
    [TestCase(10)]
    [TestCase(1000)]
    public void TestStateInt32(int value)
    {
      var state = ResolveState<StateInt32, int>();
      state.Set(value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase("hello word")]
    [TestCase("happy hacking")]
    public void TestStateString(string value)
    {
      var state = ResolveState<StateString, string>();
      state.Set(value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase(new byte[] { 0x00 })]
    [TestCase(new byte[] { 0x00, 0x01, 0x02 })]
    public void TestStateBytes(byte[] value)
    {
      var state = ResolveState<StateBytes, byte[]>();
      state.Set(value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase(1)]
    public void TestStateHook(int value)
    {
      var getHookData = 0;
      var setHookData = 0;
      var state = ResolveState<StateInt32, int>();

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
      var state = ResolveState<StateInt32, int>();

      state.Collect(0);
      state.AddGetHook(_ => state._collectInterval.Stop());
      state.Set(100);
      state._collectInterval.Start().WaitAsync().AssertFinishIn();
    }

    [Test]
    public void TestStateHearteatUnheartbeat()
    {
      var state = ResolveState<StateInt32, int>();

      state.Heartbeat(0);
      state.AddSetHook(value => state._heartbeatInterval.Stop());
      state._heartbeatInterval.Start().WaitAsync().AssertFinishIn();
    }

    [Test]
    public void TestWatcher()
    {
      var state = ResolveState<StateInt32, int>();

      state.Watch(_ => state._collectInterval.Stop());
      state.Collect(0);
      state.Set(1);
      state._collectInterval.Start().WaitAsync().AssertFinishIn();
    }
  }
}
