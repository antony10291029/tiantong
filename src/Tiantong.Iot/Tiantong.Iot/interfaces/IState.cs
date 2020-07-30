using System;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public interface IState
  {
    DateTime CurrentValueChangedAt { get; }

    int Id();

    int Length();

    string Name();

    string Address();

    bool IsReadLogOn();

    bool IsWriteLogOn();

    IState Id(int id);

    IState PlcId(int plcId);

    IState Name(string name);

    IState Address(string name);

    IState Length(int length);

    IState IsReadLogOn(bool flag);

    IState IsWriteLogOn(bool flag);

    IState OnError(Action<PlcStateError> onError);

    IState Build(IStateDriver driver);

    string Get();

    void Set(string value);

    string Collect(int interval = 1000);

    void AddGetHook(Action<string> hook);

    void AddGetHook(Action<string, string> hook);

    void AddSetHook(Action<string> hook);
  }

  public interface IState<T>: IState
  {

  }
}
