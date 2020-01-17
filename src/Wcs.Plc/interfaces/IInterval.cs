using System.Threading;
using System;
using System.Threading.Tasks;

namespace Wcs.Plc
{
  public interface IInterval
  {
    /// <summary>
    ///   <see cref="IIntervalManager.Add(IInterval)" /> 时分配的唯一 Id
    /// </summary>
    int Id { get; set; }

    IInterval SetTime(int time);

    IInterval SetTimes(int times);

    IInterval SetHandler(Action handler);

    IInterval SetHandler(Func<Task> handler);

    IInterval SetHandler(Func<CancellationToken, Task> handler);

    bool IsRunning();

    IInterval Start();

    IInterval Stop();

    Task WaitAsync();

    void Wait();

    Task RunAsync();

    void Run();
  }
}
