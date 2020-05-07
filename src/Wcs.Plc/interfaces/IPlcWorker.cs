using System;
using System.Threading.Tasks;
using Wcs.Plc.Entities;

namespace Wcs.Plc
{
  public interface IPlcWorker
  {
    PlcConnection PlcConnection { get; }

    IPlcWorker Id(int id);

    IPlcWorker Model(string key);

    IPlcWorker Name(string key);

    IPlcWorker Host(string host);

    IPlcWorker Port(int port);

    IPlcWorker Build();

    IPlcWorker UseTest();

    IPlcWorker UseS7200Smart(string host, int port = 102);

    IPlcWorker UseMC3E(string host, int port);

    IStateManager Define(string name);

    //

    IState<bool> Bool(string name);

    IState<ushort> UInt16(string name);

    IState<string> String(string name);

    IState<byte[]> Bytes(string name);

    //

    IState<ushort> UShort(string name);

    IState<int> Int(string name);

    //

    IPlcWorker Start();

    IPlcWorker Stop();

    Task WaitAsync();

    void Wait();

    Task RunAsync();

    void Run();
  }
}
