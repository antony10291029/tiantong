using System;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public interface IState
  {
    string CurrentValue { get; }

    DateTime CurrentValueChangedAt { get; }

    bool IsCollect { get; set; }

    int Id();

    int Length();

    string Name();

    string Address();

    IState Id(int id);

    IState PlcId(int plcId);

    IState Name(string name);

    IState Address(string name);

    IState Length(int length);

    IState OnError(Action<PlcStateError> onError);

    IState OnLog(Action<PlcStateLog> onLog);

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
