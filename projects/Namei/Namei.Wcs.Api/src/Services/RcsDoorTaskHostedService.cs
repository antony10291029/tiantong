using DotNetCore.CAP;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Namei.Wcs.Api
{
  // 后台托管服务
  public class DoorTaskHostedService: IntervalService
  {
    private ICapPublisher _cap;

    private IServiceProvider _services;

    private Action<DomainContext> _useDomainContext;

    public DoorTaskHostedService(ICapPublisher cap, IServiceProvider services)
    {
      _cap = cap;
      _services = services;
    }

    private void UseDomainContext(Action<DomainContext> callback)
    {
      using (var scope = _services.CreateScope())
      {
        var domain = scope.ServiceProvider.GetService<DomainContext>();

        callback(domain);
      }
    }

    private void HandleRcsTasks(DomainContext domain)
    {
      var expiredAt = DateTime.Now.AddSeconds(-10);

      var tasks = domain.Set<RcsDoorTask>()
        .Where(task => task.Status == RcsDoorTaskStatus.Entered)
        .Where(task => task.EnteredAt < expiredAt)
        .Where(task => task.RetryCount <= 3)
        .ToArray();

      foreach (var task in tasks) {
        task.Retry();
        _cap.Publish(
          RcsDoorEvent.Retry,
          RcsDoorEvent.From(
            uuid: task.Uuid,
            doorId: task.DoorId
          )
        );
      }

      if (tasks.Count() > 0) {
        domain.SaveChanges();
      }
    }

    protected override Task HandleJob(CancellationToken token)
    {
      UseDomainContext(HandleRcsTasks);

      return Task.CompletedTask;
    }
  }
}
