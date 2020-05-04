using System;
using System.Linq;
using Wcs.Plc.Entities;
using NUnit.Framework;

namespace Wcs.Plc.Test
{
  [TestFixture]
  public class PlcTest
  {
    [Test]
    public void TestTryWait()
    {
      var plc = new PlcWorker();

      plc.UseTest();
      plc.Define("hb").Int("D1").Collect(0);
      plc.Int("hb").Set(1);

      try {
        plc.Start().WaitAsync().AssertFinishIn(1);
        Assert.Fail("except to throw exception when Plc.TryWait is timeout");
      } catch {}
    }

    [Test]
    public void TestGetId()
    {
      var plc = new PlcWorker().UseTest();

      plc.Define("int", 100).Int("D1");
      plc.Int(100);
      try {
        plc.Int(101);
      } catch (Exception) {}
    }

    [Test]
    public void TestPlcCollectRunStop()
    {
      var plc = new PlcWorker();

      plc.UseTest();
      plc.Define("bool data").Bool("D1").Collect(0)
        .Watch("==", true).On(_ => plc.Stop());
      plc.Bool("bool data").Set(true);

      plc.Start().WaitAsync().AssertFinishIn();
    }

    [Test]
    public void TestAliasTypes()
    {
      var plc = new PlcWorker();

      plc.UseTest();

      Assert.IsTrue(plc.Define("ushort").UShort("D1") is IState<ushort>);
      Assert.IsTrue(plc.UShort("ushort") is IState<ushort>);

      Assert.IsTrue(plc.Define("int").Int("D2") is IState<int>);
      Assert.IsTrue(plc.Int("int") is IState<int>);
    }

    [Test]
    public void TestPlcHeartbeat()
    {
      var plc = new PlcWorker();

      plc.UseTest();
      plc.Define("hb").Int("D1").Heartbeat(1).Collect(0)
        .Watch(value => value > 1).On(_ =>  plc.Stop());

      plc.Start().WaitAsync().AssertFinishIn();
    }

  }
}
