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
      var plc = new Plc();

      plc.UseTest();
      plc.Define("hb").Int("D1").Collect(0);
      plc.Int("hb").Set(1);

      try {
        plc.Start().WaitAsync().AssertFinishIn(1);
        Assert.Fail("except to throw exception when Plc.TryWait is timeout");
      } catch {}
    }

    [Test]
    public void TestPlcCollectRunStop()
    {
      var plc = new Plc();

      plc.UseTest();
      plc.Define("bool data").Bool("D1").Collect(0)
        .Watch("==", true).On(_ => plc.Stop());
      plc.Bool("bool data").Set(true);

      plc.Start().WaitAsync().AssertFinishIn();
    }

    [Test]
    public void TestAliasTypes()
    {
      var plc = new Plc();

      plc.UseTest();

      Assert.IsTrue(plc.Define("ushort").UShort("D1") is IState<ushort>);
      Assert.IsTrue(plc.UShort("ushort") is IState<ushort>);

      Assert.IsTrue(plc.Define("int").Int("D2") is IState<int>);
      Assert.IsTrue(plc.Int("int") is IState<int>);
    }

    [Test]
    public void TestPlcHeartbeat()
    {
      var plc = new Plc();

      plc.UseTest();
      plc.Define("hb").Int("D1").Heartbeat(0).Collect(0)
        .Watch(value => value > 1).On(_ =>  plc.Stop());

      plc.Start().WaitAsync().AssertFinishIn();
    }

    [Test]
    public void TestPlcConnectionIdException()
    {
      var plc = new Plc();

      plc.Id(100);
      try {
        plc.HandlePlcConnection();
        Assert.Fail("Exception should be thrown when Id does not existed");
      } catch {}
    }

    [Test]
    public void TestPlcConnectionId()
    {
      var plc = new Plc();
      plc.ResolveDatabaseProvider();
      var db = plc.DatabaseProvider.Resolve();
      var connection = new PlcConnection {
        Id = 10,
        Name = "test",
        Model = "melsec",
        Host = "localhost",
        Port = 1234,
      };

      db.Add(connection);
      db.SaveChanges();

      plc.Id(10);
      plc.HandlePlcConnection();
      connection = plc.PlcConnection;

      Assert.AreEqual("test", connection.Name);
      Assert.AreEqual("melsec", connection.Model);
      Assert.AreEqual("localhost", connection.Host);
      Assert.AreEqual(1234, connection.Port);
    }

    [Test]
    public void TestPlcConnectionName()
    {
      var plc = new Plc();

      plc.ResolveDatabaseProvider();
      plc.Name("test").Model("melsec").Host("localhost").Port(1234);
      plc.HandlePlcConnection();

      Assert.AreNotEqual(plc.PlcConnection.Id, 0);
    }

    [Test]
    public void TestPlcConnectionNameExisted()
    {
      var plc = new Plc();
      plc.ResolveDatabaseProvider();
      var db = plc.DatabaseProvider.Resolve();
      var connection = new PlcConnection {
        Name = "test",
        Model = "melsec",
        Host = "localhost",
        Port = 1234,
      };

      plc.Name("test").Model("siemens").Host("localhost").Port(1234);
      plc.HandlePlcConnection();

      connection = db.PlcConnections.Where(item => item.Name == "test").First();

      Assert.AreEqual("siemens", connection.Model);
    }

  }
}
