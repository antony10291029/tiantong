using System;
using System.Threading.Tasks;
using Wcs.Plc.Entities;

namespace Wcs.Plc
{
  public interface IState
  {

  }

  public interface IStateBuilder<T>: IState
  {
    IStateHook<T> AddGetHook(Action<T> hook);

    IStateHook<T> AddSetHook(Action<T> hook);

    IStateBuilder<T> Watch(Action<T> handler);

    IWatcher<T> Watch();

    IWatcher<T> Watch(T value);

    IWatcher<T> Watch(Func<T, bool> cmp);

    IWatcher<T> Watch(string opt, T value);

    IStateBuilder<T> Collect(int interval = 1000);

    IStateBuilder<T> Heartbeat(int times = 100, int maxTimes = 10000);

  }

  public interface IState<T>: IState
  {
    void Set(T data);

    T Get();

    Task UncollectAsync();

    void Uncollect();

    Task UnheartbeatAsync();

    void Unheartbeat();

  }

}
