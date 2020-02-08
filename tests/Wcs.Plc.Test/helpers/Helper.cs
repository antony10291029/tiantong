using System;
using System.Threading;
using System.Threading.Tasks;

namespace Wcs.Plc.Test
{
  public static class Helper
  {
    public static void TryWait(Task task, int time)
    {
      var tokenSource = new CancellationTokenSource();
      var delay = Task.Delay(time, tokenSource.Token);

      task.ContinueWith(_ => tokenSource.Cancel());

      try {
        delay.GetAwaiter().GetResult();
        throw new Exception($"interval overtime: expect to finish task in {time}ms");
      } catch (TaskCanceledException) {}
    }

    public static void TryWait(this Interval interval, int time = 1000)
    {
      TryWait(interval.WaitAsync(), time);
    }

    public static void TryWait(this IntervalManager manager, int time = 1000)
    {
      TryWait(manager.WaitAsync(), time);
    }

    public static void TryWait(this IPlc plc, int time = 1000)
    {
      TryWait(plc.WaitAsync(), time);
    }
  }
}
