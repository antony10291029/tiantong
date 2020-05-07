using System;
using System.Threading;
using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class Interval
  {
    public int Id { get; set; }

    private int _time = 1;

    private Task _task = Task.Delay(0);

    private Func<CancellationToken, Task> _handler;

    private Func<CancellationToken, Task> _delayer;

    private CancellationTokenSource _tokenSource = new CancellationTokenSource();

    public Interval()
    {
      ResolveDelayer();
    }

    public Interval(Func<Task> handler, int time = 1000)
    {
      SetTime(time);
      SetHandler(handler);
      ResolveDelayer();
    }

    public Interval(Action handler, int time = 1000)
    {
      SetTime(time);
      SetHandler(handler);
      ResolveDelayer();
    }

    ~Interval()
    {
      if (_task != null) {
        Stop();
      }
    }

    private void ResolveDelayer()
    {
      if (_time > 0) {
        _delayer = token => Task.Delay(_time, token);
      }
    }

    public Interval UseDelayer(Func<CancellationToken, Task> delayer)
    {
      _delayer = delayer;

      return this;
    }

    public Interval SetTime(int time)
    {
      _time = Math.Max(time, 0);

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

    private async Task HandleTask()
    {
      while (!_tokenSource.Token.IsCancellationRequested) {
        await _handler(_tokenSource.Token);
        try {
          await Task.Delay(_time, _tokenSource.Token);
        } catch (TaskCanceledException) {
        } catch (Exception e) {
          Console.WriteLine(e);
          throw e;
        }
      }
    }

    public Interval Start()
    {
      _tokenSource = new CancellationTokenSource();
      _task = HandleTask();

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
