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
      var clientProvider = new StateTestClientProvider();
      var logger = new StateLogger(intervalManager, db, plcConnection);
      var manager = new StateManager(event_, intervalManager, clientProvider,logger);

      return manager;
    }

    [Test]
    public void TestManagerStates()
    {
      var manager = ResolveManager();
      var states = manager.States;

      manager.SetName("bit").Bit("D1");
      manager.SetName("bits").Bits("D2");
      manager.SetName("word").Word("D3");
      manager.SetName("words").Words("D4");

      var bit = states["bit"].ToBit();
      var bits = states["bits"].ToBits();
      var word = states["word"].ToWord();
      var words = states["words"].ToWords();

      bit.Set(true);
      bits.Set("0101");
      word.Set(100);
      words.Set("AAAA");

      Assert.AreEqual(bit.Get(), true);
      Assert.AreEqual(bits.Get(), "0101");
      Assert.AreEqual(word.Get(), 100);
      Assert.AreEqual(words.Get(), "AAAA");
    }

    [Test]
    public void TestManagerRemove()
    {
      var manager = ResolveManager();
      var states = manager.States;

      manager.SetName("bit").Bit("D1");
      states.Remove("D1");

      try {
        var state = states["D1"];
        Assert.Fail("state should be removed");
      } catch {}
    }
  }
}
