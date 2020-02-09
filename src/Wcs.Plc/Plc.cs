using System.Diagnostics;
using System;
using System.Linq;
using System.Threading.Tasks;
using Wcs.Plc.Entities;
using Microsoft.EntityFrameworkCore;

namespace Wcs.Plc
{
  public class Plc : IPlc
  {
    public EventPlugin EventLogger;

    public IStatePlugin StateLogger;

    public StateManager StateManager;

    public Event Event = new Event();

    public PlcConnection PlcConnection = new PlcConnection();

    public IntervalManager IntervalManager = new IntervalManager();

    public DatabaseProvider DatabaseProvider;

    public IStateClientProvider StateClientProvider;

    public Plc()
    {
      DatabaseProvider = ResolveDatabaseProvider();
      StateClientProvider = ResolveStateClientProvider();
      EventLogger = ResolveEventLogger();
      StateLogger = ResolveStateLogger();
      StateManager = ResolveStateManager();

      DatabaseProvider.Migrate();
    }

    //

    public virtual DatabaseProvider ResolveDatabaseProvider()
    {
      return new DatabaseProvider();
    }

    public virtual IStateClientProvider ResolveStateClientProvider()
    {
      return new StateTestClientProvider();
    }

    public virtual EventPlugin ResolveEventLogger()
    {
      var logger = new EventLogger(IntervalManager, DatabaseProvider.Resolve());

      Event.Use(logger);

      return logger;
    }

    public virtual IStatePlugin ResolveStateLogger()
    {
      return new StateLogger(IntervalManager, DatabaseProvider.Resolve(), PlcConnection);
    }

    public virtual StateManager ResolveStateManager()
    {
      return new StateManager(Event, IntervalManager, StateClientProvider, StateLogger);
    }

    public virtual void HandlePlcConnection()
    {
      var id = PlcConnection.Id;
      var name = PlcConnection.Name;
      var db = DatabaseProvider.Resolve();

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

    public IPlc Id(int id)
    {
      PlcConnection.Id = id;

      return this;
    }

    public IPlc Model(string model)
    {
      PlcConnection.Model = model;

      return this;
    }

    public IPlc Name(string name)
    {
      PlcConnection.Name = name;

      return this;
    }

    public IPlc Host(string host)
    {
      PlcConnection.Host = host;
      
      return this;
    }

    public IPlc Port(string port)
    {
      PlcConnection.Port = port;

      return this;
    }

    public IStateManager State(string name)
    {
      StateManager.Name = name;

      return StateManager;
    }

    //

    public IStateWord Word(string name)
    {
      return StateManager.States[name].ToWord();
    }

    public IStateWords Words(string name)
    {
      return StateManager.States[name].ToWords();
    }

    public IStateBit Bit(string name)
    {
      return StateManager.States[name].ToBit();
    }

    public IStateBits Bits(string name)
    {
      return StateManager.States[name].ToBits();
    }

    //

    public void On<T>(string key, Func<T, Task> handler)
    {
      Event.On<T>(key, handler);
    }

    public void On(string key, Func<Task> handler)
    {
      Event.On(key, handler);
    }

    public void On<T>(string key, Action<T> handler)
    {
      Event.On<T>(key, handler);
    }

    public void On(string key, Action handler)
    {
      Event.On(key, handler);
    }

    //

    public IPlc Start()
    {
      IntervalManager.Start();

      return this;
    }

    public IPlc Stop()
    {
      IntervalManager.Stop();

      return this;
    }

    public Task WaitAsync()
    {
      return IntervalManager.WaitAsync();
    }

    public void Wait()
    {
      IntervalManager.Wait();
    }

    public Task RunAsync()
    {
      return IntervalManager.RunAsync();
    }

    public void Run()
    {
      IntervalManager.Run();
    }
  }
}
