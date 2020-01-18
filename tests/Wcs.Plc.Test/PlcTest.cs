using System.Linq;
using Wcs.Plc.Entities;
using NUnit.Framework;

namespace Wcs.Plc.Test
{
  [TestFixture]
  public class PlcTest
  {
    [Test]
    public void TestPlcCollectRunStop()
    {
      var plc = new Plc();

      plc.State("bit data").Bit("D1").Collect(0);
      plc.Watch("bit data", "==", true).Event("event");
      plc.Bit("bit data").Set(true);
      plc.On<bool>("event", val => {
        plc.Stop();
      });

      plc.Run();
    }

    [Test]
    public void TestPlcHeartbeat()
    {
      var plc = new Plc();

      plc.State("hb").Word("D1").Heartbeat(0).Collect(0);
      plc.Watch<int>("hb", value => value > 10).Event("stop");
      plc.On<int>("stop", val => {
        plc.Stop();
      });
      plc.Run();
    }

    [Test]
    public void TestPlcConnectionIdException()
    {
      var plc = new Plc();

      plc.Id(100);
      try {
        plc.Container.ResolvePlcConnection();
        Assert.Fail("Exception should be thrown when Id does not existed");
      } catch {}
    }

    [Test]
    public void TestPlcConnectionId()
    {
      var plc = new Plc();
      var db = plc.Container.ResolveDbContext();
      var connection = new PlcConnection {
        Id = 10,
        Name = "test",
        Model = "melsec",
        Host = "localhost",
        Port = "1234",
      };

      db.Add(connection);
      db.SaveChanges();

      plc.Id(10);
      plc.Container.ResolvePlcConnection();
      connection = plc.Container.PlcConnection;

      Assert.AreEqual("test", connection.Name);
      Assert.AreEqual("melsec", connection.Model);
      Assert.AreEqual("localhost", connection.Host);
      Assert.AreEqual("1234", connection.Port);
    }

    [Test]
    public void TestPlcConnectionName()
    {
      var plc = new Plc();

      plc.Name("test").Model("melsec").Host("localhost").Port("1234");
      plc.Container.ResolvePlcConnection();

      Assert.AreNotEqual(plc.Container.PlcConnection.Id, 0);
    }

    [Test]
    public void TestPlcConnectionNameExisted()
    {
      var plc = new Plc();
      var db = plc.Container.ResolveDbContext();
      var connection = new PlcConnection {
        Name = "test",
        Model = "melsec",
        Host = "localhost",
        Port = "1234",
      };

      plc.Name("test").Model("siemens").Host("localhost").Port("1234");
      plc.Container.ResolvePlcConnection();

      connection = db.PlcConnections.Single(item => item.Name == "test");

      Assert.AreEqual("siemens", connection.Model);
    }

  }
}
