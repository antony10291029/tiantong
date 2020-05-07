using System;
using System.Threading.Tasks;
using Wcs.Plc.Entities;

namespace Wcs.Plc
{
  public interface IPlc
  {
    PlcConnection PlcConnection { get; }

    IPlc Id(int id);

    IPlc Model(string key);

    IPlc Name(string key);

    IPlc Host(string host);

    IPlc Port(int port);

    IPlc Build();

    IPlc UseTest();

    IPlc UseS7200Smart(string host, int port = 102);

    IPlc UseMC3E(string host, int port);

    IStateManager State(string key);

    IStateBool Bool(string key);

    IStateInt Int(string key);

    IStateString String(string key);

    void On<T>(string key, Func<T, Task> handler);

    void On(string key, Func<Task> handler);

    void On<T>(string key, Action<T> handler);

    void On(string key, Action handler);

    IPlc Start();

    IPlc Stop();

    Task WaitAsync();

    void Wait();

    Task RunAsync();

    void Run();
  }
}
