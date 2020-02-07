using System;
using System.Threading.Tasks;

namespace Wcs.Plc
{
  public interface IPlc
  {
    IPlc Id(int id);

    IPlc Model(string key);

    IPlc Name(string key);

    IPlc Host(string host);

    IPlc Port(string port);

    IStateManager State(string key);

    IStateWord Word(string key);

    IStateWords Words(string key);

    IStateBit Bit(string key);

    IStateBits Bits(string key);

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
