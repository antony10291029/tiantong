using System;

namespace Tiantong.Iot
{
  public interface IState
  {
    IState Collect(int interval = 1000);

    IState Heartbeat(int interval = 1000, int maxValue = 10000);
  }

  public interface IState<T>: IState
  {
    IState<T> Use(IStatePlugin plugin);

    IState<T> Id(int id);

    IState<T> Name(string name);

    IState<T> Address(string name);

    IState<T> Length(int length);

    IState<T> Build();

    void AddGetHook(Action<T> hook);

    void AddSetHook(Action<T> hook);

    IState<T> Watch(Action<T> handler);

    IWatcher<T> Watch();

    IWatcher<T> Watch(T value);

    IWatcher<T> Watch(Func<T, bool> cmp);

    IWatcher<T> Watch(string opt, T value);

    void Set(T data);

    T Get();

  }

}
