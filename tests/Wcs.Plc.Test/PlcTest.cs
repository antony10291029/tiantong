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
      plc.State("hb").Int("D1").Collect(0);
      plc.Int("hb").Set(1);

      try {
        plc.Start().TryWait(1);
        Assert.Fail("except to throw exception when Plc.TryWait is timeout");
      } catch {}
    }

    [Test]
    public void TestPlcCollectRunStop()
    {
      var plc = new Plc();

      plc.UseTest();
      plc.State("bool data").Bool("D1").Collect(0);
      plc.Bool("bool data").Watch("==", true).Event("event");
      plc.Bool("bool data").Set(true);
      plc.On<bool>("event", _ => {
        plc.Stop();
      });

      plc.Start().TryWait();
    }

    [Test]
    public void TestPlcHeartbeat()
    {
      var plc = new Plc();

      plc.UseTest();
      plc.State("hb").Int("D1").Heartbeat(0).Collect(0);
      plc.Int("hb").Watch(value => value > 1).Event("stop");
      plc.On<int>("stop", _ => {
        plc.Stop();
      });

      plc.Start().TryWait();
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
