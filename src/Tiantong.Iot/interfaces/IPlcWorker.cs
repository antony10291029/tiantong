using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Tiantong.Iot
{
  public interface IPlcWorker
  {
    int _id { get; set; }

    string _name { get; set; }

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

    IPlcWorker Heartbeat(string name, int interval = 1000, int maxValue = 10000);

    IPlcWorker Collect<T>(string name, int interval = 1000);

    IStateHttpPusher HttpPusher<T>(string name);

    void Watch(string name, Action<string> handler);

    void Watch<T>(string name, Action<T> handler);

    //

    IState State(int id);

    IState<T> State<T>(int id);

    IState State(string name);

    IState<T> State<T>(string name);

    T Get<T>(int id);

    void Set<T>(int id, T value);

    T Get<T>(string name);

    void Set<T>(string name, T value);

    //

    void Test();

    IPlcWorker Start();

    IPlcWorker Stop();

    Task WaitAsync();

    void Wait();

    Task RunAsync();

    void Run();

  }

}
