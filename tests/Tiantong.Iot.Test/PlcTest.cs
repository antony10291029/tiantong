using System.Threading.Tasks;
using System;
using System.Linq;
using Tiantong.Iot.Entities;
using NUnit.Framework;

namespace Tiantong.Iot.Test
{
  [TestFixture]
  public class PlcTest
  {
    [Test]
    public void TestTryWait()
    {
      var plc = new PlcWorker();

      plc.Config();
      plc.Define("hb").Int("D1", state => state.Collect(1));
      plc.Int("hb").Set(1);

      try {
        plc.Start().WaitAsync().AssertFinishIn(10);
        Assert.Fail("except to throw exception when Plc.TryWait is timeout");
      } catch {}
    }

    [Test]
    public void TestRun()
    {
      var plc = new PlcWorker();

      plc.Config();

      _ = plc.RunAsync();
      plc.Stop();
      plc.WaitAsync().AssertFinishIn(100);
    }

    [Test]
    public void TestGetId()
    {
      var plc = new PlcWorker();

      plc.Config(_ => {});
      plc.Define("int", 100).Int("D1", _ => {});
      plc.Int(100);
      try {
        plc.Int(101);
      } catch (Exception) {}
    }

    [Test]
    public void TestPlcCollectRunStop()
    {
      var plc = new PlcWorker();

      plc.Config(_ => {});
      plc.Define("bool data").Bool("D1", state => {
        state.Collect(0);
        state.When("==", true.ToString()).On(() => Task.Run(plc.Stop));
      });
      plc.Bool("bool data").Set(true);

      plc.Start().WaitAsync().AssertFinishIn();
    }

    [Test]
    public void TestAliasTypes()
    {
      var plc = new PlcWorker();

      plc.Config(_ => {});
      plc.Define("ushort").UShort("D1", _ => {});
      Assert.IsTrue(plc.UShort("ushort") is IState<ushort>);

      plc.Define("int").Int("D2", _ => {});
      Assert.IsTrue(plc.Int("int") is IState<int>);
    }

    [Test]
    public void TestPlcHeartbeat()
    {
      var plc = new PlcWorker();

      plc.Config(_ => {});
      plc.Define("hb").Int("D1", state => {
        state.Heartbeat(1);
      });

      plc.Start();
      Task.Delay(10).GetAwaiter().GetResult();
      Assert.IsTrue(plc.Int("hb").Get() > 0);
    }

  }
}
