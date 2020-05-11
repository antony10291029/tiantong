using System;

namespace Tiantong.Iot
{
  public interface IState
  {
    DateTime CurrentValueChangedAt { get; }

    string GetCurrentValue(int timeGapMilliseconds = 1500);

    IState Collect(int interval = 1000);

    IState Heartbeat(int interval = 1000, int maxValue = 10000);

    IState Id(int id);

    IState Name(string name);

    IState Address(string name);

    IState Length(int length);

    IState Build();

    IState Use(IStatePlugin plugin);

    void Watch(Action handler);

    void Watch(Action<string> handler);

    IWatcher When(string opt, string value);

  }

  public interface IState<T>: IState
  {
    void AddGetHook(Action<T> hook);

    void AddSetHook(Action<T> hook);

    void Set(T data);

    T Get();

  }

}
