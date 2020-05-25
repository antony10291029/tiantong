using System;

namespace Tiantong.Iot
{
  public interface IState
  {
    int _id { get; }

    DateTime CurrentValueChangedAt { get; }

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

    string CollectString(int interval = 1000);

    void AddGetHook(Action<string> hook);
  }

  public interface IState<T>: IState
  {
    void AddGetHook(Action<T> hook);

    void AddSetHook(Action<T> hook);

    T Get();

    void Set(T data);

    T Collect(int cacheInterval);

  }

}
