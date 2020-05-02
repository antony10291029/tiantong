using System;
using System.Threading.Tasks;

namespace Wcs.Plc
{
  public interface IState
  {

  }

  public interface IState<T> : IState
  {
    IStateHook<T> AddGetHook(Action<T> hook);

    IStateHook<T> AddSetHook(Action<T> hook);

    IState<T> Collect(int interval = 1000);

    Task UncollectAsync();

    void Uncollect();

    void Watch(Action<T> handler);

    IWatcher<T> Watch();

    IWatcher<T> Watch(T value);

    IWatcher<T> Watch(Func<T, bool> cmp);

    IWatcher<T> Watch(string opt, T value);

    void On(string key, Func<T, Task> handler);

    void On(string key, Action<T> handler);

    void Set(T data);

    T Get();
  }
}
