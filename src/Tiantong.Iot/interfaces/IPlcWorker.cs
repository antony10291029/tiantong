using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Tiantong.Iot
{
  public interface IPlcWorker
  {
    IPlcWorkerLogger Logger { get; }

    Dictionary<string, string> GetCurrentStateValues();

    IPlcWorker Config(Action<IPlcWorker> configer);

    IPlcWorker Id(int id);

    IPlcWorker Model(string key);

    IPlcWorker Name(string key);

    IPlcWorker Host(string host, int port = 0);

    IPlcWorker Port(int port);

    IPlcWorker Build();

    IPlcWorker UseTest();

    IPlcWorker UseS7200Smart(string host, int port = 102);

    IPlcWorker UseMC3E(string host, int port);

    IStateManager Define(string name, int id = 0);

    //

    IState<bool> Bool(string name);

    IState<ushort> UInt16(string name);

    IState<string> String(string name);

    IState<byte[]> Bytes(string name);

    //

    IState<ushort> UShort(string name);

    IState<int> Int(string name);

    //

    //

    IState<bool> Bool(int id);

    IState<ushort> UInt16(int id);

    IState<string> String(int id);

    IState<byte[]> Bytes(int id);

    //

    IState<ushort> UShort(int id);

    IState<int> Int(int id);

    //

    bool Test();

    IPlcWorker Start();

    IPlcWorker Stop();

    Task WaitAsync();

    void Wait();

    void Run();
  }
}
