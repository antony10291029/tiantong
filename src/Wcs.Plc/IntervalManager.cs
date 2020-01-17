using System.Threading.Tasks;
using System.Collections.Generic;

namespace Wcs.Plc
{
  using Tasks = List<Task>;
  using Intervals = Dictionary<int, IInterval>;

  public class IntervalManager : IIntervalManager
  {
    private int _id = 0;

    private Intervals intervals = new Intervals();

    public void Add(IInterval interval)
    {
      var id = _id++;
      interval.Id = id;
      intervals.Add(id, interval);
    }

    public void Remove(IInterval interval)
    {
      var id = interval.Id;
      interval.Stop();
      interval.WaitAsync().ContinueWith(_ => {
        interval.Id = 0;
        intervals.Remove(id);
      });
    }

    public bool IsRunning()
    {
      foreach (var interval in intervals.Values) {
        if (interval.IsRunning()) {
          return true;
        }
      }

      return false;
    }

    public IIntervalManager Start()
    {
      foreach (var interval in intervals.Values) {
        interval.Start();
      }

      return this;
    }

    public IIntervalManager Stop()
    {
      foreach (var interval in intervals.Values) {
        if (interval.IsRunning()) {
          interval.Stop();
        }
      }

      return this;
    }

    public IIntervalManager Clear()
    {
      foreach (var interval in intervals.Values) {
        Remove(interval);
      }

      return this;
    }

    public Task WaitAsync()
    {
      var tasks = new Tasks();

      foreach (var interval in intervals.Values) {
        tasks.Add(interval.WaitAsync());
      }

      return Task.WhenAll(tasks);
    }

    public void Wait()
    {
      WaitAsync().GetAwaiter().GetResult();
    }

    public Task RunAsync()
    {
      return Start().WaitAsync();
    }

    public void Run()
    {
      Start().Wait();
    }
  }
}
