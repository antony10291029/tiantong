using System;

namespace Tiantong.Iot
{
  public interface IState
  {
    int _id { get; }

    DateTime CurrentValueChangedAt { get; }

    string GetCurrentValue(int timeGapMilliseconds = 1500);

    IState Collect(int interval = 1000);

    IState Heartbeat(int interval = 1000, int maxValue = 10000);

    IState Id(int id);

    IState PlcId(int plcId);

    IState Name(string name);

    IState Address(string name);

    IState Length(int length);

    IState IsReadLogOn(bool flag);

    IState IsWriteLogOn(bool flag);

    IState Build();

    IState UseErrorLogger(StateErrorLogger logger);

    void SetString(string value);

    void Watch(Action handler);

    void Watch(Action<string> handler);

    IWatcher When(string opt, string value);

    IStateHttpPusher HttpPusher();

  }

  public interface IState<T>: IState
  {
    void AddGetHook(Action<T> hook);

    void AddSetHook(Action<T> hook);

    void Set(T data);

    T Get();

  }

}
