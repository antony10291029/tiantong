using System;
using System.Threading.Tasks;
using Wcs.Plc.Entities;

namespace Wcs.Plc
{
  public interface IPlc
  {
    PlcConnection PlcConnection { get; }

    IPlc Id(int id);

    IPlc Model(string key);

    IPlc Name(string key);

    IPlc Host(string host);

    IPlc Port(int port);

    IPlc Build();

    IPlc UseTest();

    IPlc UseS7200Smart(string host, int port = 102);

    IPlc UseMC3E(string host, int port);

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

    IPlc Start();

    IPlc Stop();

    Task WaitAsync();

    void Wait();

    Task RunAsync();

    void Run();
  }
}
