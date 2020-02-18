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
      var client = new StateTestClient() {
        Store = new StateTestClientStore()
      };

      state.StateClient = client;
      state.Event = new Event();
      state.IntervalManager = new IntervalManager();
      state.Length = 1;
      state.Key = "D100";
      state.Name = "test";

      return state;
    }

    [Test]
    public void TestConversation()
    {
      var types = new [] { "Bool", "Int", "String" };
      var states = new State[] { new StateBool(), new StateInt(), new StateString() };

      foreach (var state in states) {
        foreach (var type in types) {
          try {
            try {
              typeof(State).GetMethod($"ToState{type}").Invoke(state, null);
            } catch (TargetInvocationException ex) { throw ex.GetBaseException(); }
            if (state.Type != type) {
              Assert.Fail($"convert state from {state.Type} to {type} should be failed");
            }
          } catch (StateConversationException ex) {
            Assert.AreEqual(ex.To, type);
            Assert.AreEqual(ex.From, state.Type);
          }
        }
      }
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
      Assert.AreEqual(value, setHookData);

      state.AddGetHook(data => getHookData = data);
      state.Get();
      Assert.AreEqual(value, getHookData);
    }

    [Test]
    public void TestStateCollectAndUncollect()
    {
      var state = ResolveState<StateInt>();

      state.Collect(0);
      state.AddGetHook(_ => state.UncollectAsync());
      state.Set(100);
      state.IntervalManager.Start().TryWait();
    }

    [Test]
    public void TestStateHearteatUnheartbeat()
    {
      var state = ResolveState<StateInt>();
      var manager = state.IntervalManager;

      state.Heartbeat(0);
      state.AddSetHook(value => state.UnheartbeatAsync());
      manager.Start().TryWait();
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
      state.IntervalManager.Start().TryWait();
    }
  }
}
