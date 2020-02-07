using System;
using System.Linq;
using Wcs.Plc.Database;
using Wcs.Plc.Entities;
using Wcs.Plc.DB.Sqlite;

namespace Wcs.Plc
{
  public class PlcContainer
  {
    public IPlc Plc { get; set; }

    public Event Event { get; set; }

    public IStateDriver StateDriver { get; set; }

    public IStatePlugin StateLogger { get; set; }

    public IStateManager StateManager { get; set; }

    public PlcConnection PlcConnection { get; set; }

    public IntervalManager IntervalManager { get; set; }

    public IStateDriverProvider StateDriverProvider { get; set; }

    public PlcContainer()
    {
      Event = new Event();
      PlcConnection = new PlcConnection();
      StateManager = new StateManager(this);
      IntervalManager = new IntervalManager();
      StateDriverProvider = ResolveStateDriverProvider();

      UseEventLogger();
      UseStateLogger();
    }

    public virtual DbContext ResolveDbContext()
    {
      return new SqliteDbContext();
    }

    public virtual IStateDriverProvider ResolveStateDriverProvider()
    {
      return new StateTestDriverProvider();
    }

    public virtual void UseEventLogger()
    {
      var logger = new EventLogger(this);

      Event.Use(logger);
    }

    public virtual void UseStateLogger()
    {
      var logger = new StateLogger(this);

      StateLogger = logger;
    }

    public virtual void ResolvePlcConnection()
    {
      var db = ResolveDbContext();
      var id = PlcConnection.Id;
      var name = PlcConnection.Name;

      if (id != 0) {
        var conn = db.PlcConnections.SingleOrDefault(item => item.Id == id);

        if (conn != null) {
          PlcConnection = conn;
        } else {
          throw new Exception($"PlcConnection Id({id}) does not existed");
        }
      } else if (name != null) {
        var conn = db.PlcConnections.SingleOrDefault(item => item.Name == name);

        if (conn == null) {
          db.PlcConnections.Add(PlcConnection);
        } else {
          conn.Model = PlcConnection.Model;
          conn.Host = PlcConnection.Host;
          conn.Port = PlcConnection.Port;
          PlcConnection = conn;
        }

        db.SaveChanges();
      } else {
        throw new Exception("Plc Connection Id or Name does not existed");
      }
    }

  }
}
