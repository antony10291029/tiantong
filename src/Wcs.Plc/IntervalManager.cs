using System.Threading.Tasks;
using System.Collections.Generic;

namespace Wcs.Plc
{
  using Tasks = List<Task>;
  using Intervals = Dictionary<int, Interval>;

  public class IntervalManager
  {
    private int _id = 0;

    private Intervals intervals = new Intervals();

    public void Add(Interval interval)
    {
      var id = _id++;
      interval.Id = id;
      intervals.Add(id, interval);
    }

    public void Remove(Interval interval)
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

    public IntervalManager Start()
    {
      foreach (var interval in intervals.Values) {
        interval.Start();
      }

      return this;
    }

    public IntervalManager Stop()
    {
      foreach (var interval in intervals.Values) {
        if (interval.IsRunning()) {
          interval.Stop();
        }
      }

      return this;
    }

    public IntervalManager Clear()
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
