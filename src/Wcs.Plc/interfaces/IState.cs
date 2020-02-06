using System;
using System.Threading.Tasks;

namespace Wcs.Plc
{
  public interface IState
  {
    String Name { get; set; }

    String Key { get; set; }

    int Length { get; set; }

    IState Collect(int interval = 1000);

    Task UncollectAsync();

    void Uncollect();

    S Convert<S>() where S : IState;
  }

  public interface IState<T> : IState where T : IComparable
  {
    IStateHook<T> AddGetHook(Action<T> hook);

    IStateHook<T> AddSetHook(Action<T> hook);

    IWatcher<T> Watch();

    IWatcher<T> Watch(T value);

    IWatcher<T> Watch(Func<T, bool> cmp);

    IWatcher<T> Watch(string opt, T value);

    Task SetAsync(T data);

    void Set(T data);

    Task<T> GetAsync();

    T Get();
  }
}
