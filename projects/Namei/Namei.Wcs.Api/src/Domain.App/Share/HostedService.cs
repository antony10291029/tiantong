using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Namei.Wcs.Api
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
        } catch (Exception e) {
          Console.WriteLine(e);
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

    protected abstract Task HandleJob(CancellationToken token);

    public async Task StartAsync(CancellationToken token)
    {
      _stoppingToken = new CancellationTokenSource();

      _task = RunAsync(_stoppingToken.Token);

      await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken token)
    {
      _stoppingToken.Cancel();

      await Task.CompletedTask;
    }

    public virtual void Dispose()
    {

    }
  }
}
