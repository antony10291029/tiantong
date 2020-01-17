using System.Threading.Tasks;

namespace Wcs.Plc
{
  public interface IIntervalManager
  {
    void Add(IInterval interval);

    void Remove(IInterval interval);

    bool IsRunning();

    IIntervalManager Start();

    IIntervalManager Stop();

    IIntervalManager Clear();

    Task WaitAsync();

    void Wait();

    Task RunAsync();

    void Run();
  }
}
