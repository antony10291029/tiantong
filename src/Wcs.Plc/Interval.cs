using System;
using System.Threading;
using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class Interval
  {
    public int Id { get; set; }

    public int Times { get; protected set; } = 0;

    private int _time = 1;

    private Task _task = Task.Delay(0);

    private Func<CancellationToken, Task> _handler;

    private CancellationTokenSource _tokenSource = new CancellationTokenSource();

    public Interval()
    {

    }

    ~Interval()
    {
      if (_task != null) {
        Stop();
      }
    }

    public Interval(Func<Task> handler, int time = 500)
    {
      SetTime(time);
      SetHandler(handler);
    }

    public Interval(Action handler, int time = 500)
    {
      SetTime(time);
      SetHandler(handler);
    }

    public Interval SetTime(int time)
    {
      _time = Math.Max(time, 1);

      return this;
    }

    public Interval SetTimes(int times)
    {
      Times = times;

      return this;
    }

    public Interval SetHandler(Action handler)
    {
      _handler = _ => Task.Run(handler);

      return this;
    }

    public Interval SetHandler(Func<Task> handler)
    {
      _handler = _ => handler();

      return this;
    }

    public Interval SetHandler(Func<CancellationToken, Task> handler)
    {
      _handler = handler;

      return this;
    }

    public bool IsRunning()
    {
      return _task != null;
    }

    private async Task RunTask()
    {
      var times = 0;

      while (!_tokenSource.Token.IsCancellationRequested) {
        if (Times != 0) {
          if (times < Times) times++;
          else break;
        }
        try {
          await _handler(_tokenSource.Token);
        } catch (Exception e) {
          Console.WriteLine(e);
          break;
        }
        try {
          await Task.Delay(_time, _tokenSource.Token);
        } catch (TaskCanceledException) {}
      }
    }

    public Interval Start()
    {
      _tokenSource = new CancellationTokenSource();
      _task = RunTask();

      return this;
    }

    public Interval Stop()
    {
      _tokenSource.Cancel();

      return this;
    }

    public Task WaitAsync()
    {
      return _task;
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
