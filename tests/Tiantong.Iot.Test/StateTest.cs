using System.Threading.Tasks;
using NUnit.Framework;

namespace Tiantong.Iot.Test
{
  [TestFixture]
  public class StateTest
  {
    public T ResolveState<T, U>() where T : State<U>, new()
    {
      var state = new T();
      var driver = new StateTestDriverProvider().Resolve();

      state.Name("test").Address("D100").Build(driver);

      return state;
    }

    [TestCase(true)]
    public void TestStateBool(bool value)
    {
      var state = ResolveState<StateBool, bool>();
      state.Set(state.ToString(value));
      var result = state.Get();

      Assert.AreEqual(state.ToString(value), result);
    }

    [TestCase(0)]
    [TestCase(10)]
    [TestCase(1000)]
    public void TestStateUInt16(int value)
    {
      var state = ResolveState<StateUInt16, ushort>();
      state.Set(state.ToString((ushort) value));
      var result = state.Get();

      Assert.AreEqual(state.ToString((ushort) value), result);
    }

    [TestCase(0)]
    [TestCase(10)]
    [TestCase(1000)]
    public void TestStateInt32(int value)
    {
      var state = ResolveState<StateInt32, int>();
      state.Set(state.ToString(value));
      var result = state.Get();

      Assert.AreEqual(state.ToString(value), result);
    }

    [TestCase("hello word")]
    [TestCase("happy hacking")]
    public void TestStateString(string value)
    {
      var state = ResolveState<StateString, string>();
      state.Set(value);
      var result = state.Get();

      Assert.AreEqual(state.ToString(value), result);
    }

    [TestCase(1)]
    public void TestStateHook(int value)
    {
      var getHookData = "0";
      var setHookData = "0";
      var state = ResolveState<StateInt32, int>();

      state.AddSetHook(data => setHookData = data);
      state.Set(state.ToString(value));
      Task.Delay(3).GetAwaiter().GetResult();
      Assert.AreEqual(state.ToString(value), setHookData);

      state.AddGetHook(data => getHookData = data);
      state.Get();
      Task.Delay(3).GetAwaiter().GetResult();
      Assert.AreEqual(state.ToString(value), getHookData);
    }
  }
}
