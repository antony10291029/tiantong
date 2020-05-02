using System;
using System.Threading;
using System.Threading.Tasks;

namespace Wcs.Plc.Test
{
  public static class Helper
  {
    public static void AssertFinishIn(this Task task, int time = 1000)
    {
      var tokenSource = new CancellationTokenSource();
      var delay = Task.Delay(time, tokenSource.Token);
      var task_ = task.ContinueWith(_ => tokenSource.Cancel());

      try {
        delay.GetAwaiter().GetResult();
        throw new Exception($"interval overtime: expect to finish task in {time}ms");
      } catch (TaskCanceledException) {}

      task.GetAwaiter().GetResult();
    }

  }
}
