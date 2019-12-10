using NUnit.Framework;

namespace Wcs.Plc.Test
{
  [TestFixture]
  public class StateTest
  {
    private void SetState(IState state)
    {
      state.Key = "D100";
      state.Length = 1;
    }

    [TestCase(0)]
    public void TestWordState(int value)
    {
      var container = Container.GetTestContainer();
      var state = new StateWord(container);

      SetState(state);
      state.Set(value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase("happy hacking")]
    public void TestWordsState(string value)
    {
      var container = Container.GetTestContainer();
      var state = new StateWords(container);

      SetState(state);
      state.Set(value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase(true)]
    public void TestBitState(bool value)
    {
      var container = Container.GetTestContainer();
      var state = new StateBit(container);

      SetState(state);
      state.Set(value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase("0011")]
    public void TestBitsState(string value)
    {
      var container = Container.GetTestContainer();
      var state = new StateBits(container);

      SetState(state);
      state.Set(value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase(1)]
    public void TestStateHook(int value)
    {
      var getHookData = 0;
      var setHookData = 0;
      var container = Container.GetTestContainer();
      var state = new StateWord(container);

      SetState(state);
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
      var flag = false;
      var container = Container.GetTestContainer();
      var state = new StateWord(container);
      var manager = container.IntervalManager;

      SetState(state);
      state.Collect(0);
      state.AddGetHook(value => {
        flag = true;
        state.UncollectAsync();
      });
      state.Set(100);
      manager.Start();
      manager.Wait();
      Assert.IsTrue(flag);
    }

    [Test]
    public void TestStateHearteatUnheartbeat()
    {
      var container = Container.GetTestContainer();
      var state = new StateWord(container);
      var manager = container.IntervalManager;

      SetState(state);
      state.Heartbeat(0);
      state.AddSetHook(value => {
        if (value > 10) {
          state.UnheartbeatAsync();
        }
      });
      manager.Start().Wait();
    }
  }
}
