using NUnit.Framework;

namespace Wcs.Plc.Test
{
  [TestFixture]
  public class PlcTest
  {
    [Test]
    public void TestPlcCollectRunStop()
    {
      var plc = Plc.GetWorker();

      plc.State("bit data").Bit("D1").Collect(0);
      plc.Watch("bit data", "==", true).Event("event");
      plc.Bit("bit data").Set(true);
      plc.On<bool>("event", _ => {
        plc.StopAsync();
      });

      plc.Run();
    }

    [Test]
    public void TestPlcHeartbeat()
    {
      var plc = Plc.GetWorker();

      plc.State("hb").Word("D1").Heartbeat(0).Collect(0);
      plc.Watch<int>("hb", value => value > 10).Event("stop");
      plc.On<int>("stop", _ => {
        plc.StopAsync();
      });
      plc.Run();
    }
  }
}
