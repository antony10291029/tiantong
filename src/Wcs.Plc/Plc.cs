using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wcs.Plc.Entities;

namespace Wcs.Plc
{
  public class Plc : IPlc
  {
    public Event Event;

    public EventPlugin EventLogger;

    public IStatePlugin StateLogger;

    public StateManager StateManager;

    public IntervalManager IntervalManager;

    public DatabaseProvider DatabaseProvider;

    public IStateDriverProvider StateDriverProvider;

    public PlcConnection PlcConnection { get; set; }

    public Plc()
    {
      PlcConnection = new PlcConnection();
    }

    //

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

    public IPlc Port(int port)
    {
      PlcConnection.Port = port;

      return this;
    }

    //

    public virtual void ResolveDatabaseProvider()
    {
      DatabaseProvider = new DatabaseProvider();
      DatabaseProvider.Migrate();
    }

    public virtual void ResolveStateDriverProvider()
    {
      StateDriverProvider = PlcConnection.Model switch {
        "test" => new StateTestDriverProvider(),
        "MC3E" => new MC3EDriverProvider(PlcConnection.Host, PlcConnection.Port),
        "S7200Smart"=> new S7200SmartDriverProvider(PlcConnection.Host, PlcConnection.Port),
        _ => throw new Exception("plc model is not supporting"),
      };
    }

    //

    public IPlc Build()
    {
      Event = new Event();
      IntervalManager = new IntervalManager();
      ResolveDatabaseProvider();
      ResolveStateDriverProvider();
      ResolveEventLogger();
      ResolveStateLogger();
      ResolveStateManager();

      HandlePlcConnection();

      return this;
    }

    public virtual IPlc UseTest()
    {
      return Build();
    }

    public virtual IPlc UseS7200Smart(string host, int port = 102)
    {
      Model("S7200Smart").Host(host).Port(port).Build();

      return this;
    }

    public virtual IPlc UseMC3E(string host, int port)
    {
      Model("MC3E").Host(host).Port(port).Build();

      return this;
    }

    public virtual void ResolveEventLogger()
    {
      EventLogger = new EventLogger(IntervalManager, DatabaseProvider.Resolve());

      Event.Use(EventLogger);
    }

    public virtual void ResolveStateLogger()
    {
      StateLogger = new StateLogger(IntervalManager, DatabaseProvider.Resolve(), PlcConnection);
    }

    public virtual void ResolveStateManager()
    {
      StateManager = new StateManager(Event, IntervalManager, StateDriverProvider, StateLogger);
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
      } else {
        throw new Exception("Plc Connection Id or Name is required");
      }

      // if (PlcConnection.IsRunning) {
      //   throw new Exception($"PlcConnection Name({name}) is running");
      // }

      // PlcConnection.IsRunning = true;
      db.SaveChanges();
    }

    public IStateManager Define(string name)
    {
      StateManager.Name = name;

      return StateManager;
    }

    //

    public IStateBool Bool(string name)
    {
      return (IStateBool) StateManager.States[name];
    }

    public IStateUInt16 UInt16(string name)
    {
      return (IStateUInt16) StateManager.States[name];
    }

    public IStateInt32 Int32(string name)
    {
      return (IStateInt32) StateManager.States[name];
    }

    public IStateString String(string name)
    {
      return (IStateString) StateManager.States[name];
    }

    public IStateBytes Bytes(string name)
    {
      return (IStateBytes) StateManager.States[name];
    }

    //

    public IStateUInt16 UShort(string name)
    {
      return UInt16(name);
    }

    public IStateInt32 Int(string name)
    {
      return Int32(name);
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
      StateDriverProvider.Boot();
      IntervalManager.Start();

      return this;
    }

    public IPlc Stop()
    {
      IntervalManager.Stop();
      DatabaseProvider.Resolve().SaveChanges();

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
      return Start().WaitAsync();
    }

    public void Run()
    {
      while (true) {
        try {
          RunAsync().GetAwaiter().GetResult();
        } catch (Exception e) {
          Console.WriteLine(e.Message);
          Task.Delay(1000).GetAwaiter().GetResult();
        }
      }
    }

  }
}
