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

    IState<T> Watch(Action<T> handler);

    IWatcher<T> Watch();

    IWatcher<T> Watch(T value);

    IWatcher<T> Watch(Func<T, bool> cmp);

    IWatcher<T> Watch(string opt, T value);

    void Set(T data);

    T Get();
  }
}
