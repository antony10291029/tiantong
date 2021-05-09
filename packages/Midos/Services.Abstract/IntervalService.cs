using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Midos.Services.Abstract
{
  public abstract class IntervalService: IHostedService, IDisposable
  {
    private CancellationTokenSource _stoppingToken;

    protected int Time = 5000;

    private Task _task;

    private async Task RunAsync(CancellationToken stoppingToken)
    {
      while (true) {
        try {
          await HandleJob(stoppingToken);
        } catch(TaskCanceledException) {
        } catch (Exception e) {
          // @Todo: handle exception with logger
          Console.WriteLine(e);

          throw;
        }

        if (stoppingToken.IsCancellationRequested) {
          return;
        }

        try {
          await Task.Delay(Time, stoppingToken);
        } catch (TaskCanceledException) {
          return;
        }
      }
    }

    protected abstract Task HandleJob(CancellationToken stoppingToken);

    public async Task StartAsync(CancellationToken _)
    {
      _stoppingToken = new CancellationTokenSource();

      _task = RunAsync(_stoppingToken.Token);

      await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken _)
    {
      _stoppingToken.Cancel();

      await _task;
    }

    public virtual void Dispose()
    {
      GC.SuppressFinalize(this);
    }
  }
}
