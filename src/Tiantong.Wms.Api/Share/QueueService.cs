using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.Extensions.Hosting;

namespace Tiantong.Wms.Api
{
  public abstract class QueueService : BackgroundService
  {
    protected TaskQueue TaskQueue { get; }

    public QueueService(TaskQueue taskQueue)
    {
      TaskQueue = taskQueue;
    }

    protected void Enqueue(Func<CancellationToken, Task> task)
    {
      TaskQueue.Enqueue(task);
    }

    //

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      await BackgroundProcessing(stoppingToken);
    }

    private async Task BackgroundProcessing(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested) {
        var workItem = await TaskQueue.DequeueAsync(stoppingToken);

        try {
          await workItem(stoppingToken);
        } catch (Exception ex) {
          throw ex;
        }
      }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
      await base.StopAsync(stoppingToken);
    }
  }

  public class TaskQueue
  {
    private ConcurrentQueue<Func<CancellationToken, Task>> _workItems = 
      new ConcurrentQueue<Func<CancellationToken, Task>>();

    private SemaphoreSlim _signal = new SemaphoreSlim(0);

    public void Enqueue(Func<CancellationToken, Task> workItem)
    {
      if (workItem == null) {
        throw new ArgumentNullException(nameof(workItem));
      }

      _workItems.Enqueue(workItem);
      _signal.Release();
    }

    public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
    {
      await _signal.WaitAsync(cancellationToken);
      _workItems.TryDequeue(out var workItem);

      return workItem;
    }
  }
}
