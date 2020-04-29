using System.Threading.Tasks;
using NUnit.Framework;

namespace Wcs.Plc.Test
{
  [TestFixture]
  public class PlcManagerTest
  {
    [TestCase]
    public void TestStartStopWait()
    {
      var manager = new PlcManager();

      for (var i = 0; i < 10; i++) {
        var plc = new Plc();
        plc.Name(i.ToString()).UseTest();
        plc.Start();

        manager.Plcs.Add(plc.PlcConnection.Name, plc);
      }

      manager.Start();
      manager.Stop().WaitAsync().AssertFinishIn();
    }

    [TestCase]
    public void TestPlcItem()
    {
      var manager = new PlcManager();
      var plc = new Plc();

      plc.Name("test plc").UseTest();
      plc.State("hb").Int("D100").Heartbeat(1);
      manager.Plcs.Add(plc.PlcConnection.Name, plc);

      plc.Start();
      Task.Delay(1).ContinueWith(task => plc.Stop());
      manager.WaitAsync().AssertFinishIn(100);
    }
  }
}
