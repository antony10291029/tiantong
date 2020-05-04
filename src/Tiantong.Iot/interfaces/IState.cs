using System;
using System.Threading.Tasks;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public interface IState
  {

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

    IState<T> Collect(int interval = 1000);

    IState<T> Heartbeat(int interval = 1000);

    IState<T> HeartbeatMaxValue(T maxValue);

    void Set(T data);

    T Get();

  }

}
