using System.Reflection;
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

    private void UseContext(Action<DomainContext, WcsDoorFactory> callback)
    {
      using (var scope = _services.CreateScope())
      {
        var domain = scope.ServiceProvider.GetService<DomainContext>();
        var doors = scope.ServiceProvider.GetService<WcsDoorFactory>();

        callback(domain, doors);
      }
    }

    private void HandleRcsTasks(DomainContext domain)
    {
      var expiredAt = DateTime.Now.AddSeconds(-10);

      var tasks = domain.Set<RcsDoorTask>()
        .Where(task => task.Status == RcsDoorTaskStatus.Entered)
        .Where(task => task.EnteredAt < expiredAt)
        .ToArray();

      foreach (var task in tasks) {
        var @event = RcsDoorEvent.From(
          uuid: task.Id,
          doorId: task.DoorId
        );

        if (task.RetryCount < 2) {
          task.Retry();
          _cap.Publish(RcsDoorEvent.Retry, @event);
        } else {
          _cap.Publish(RcsDoorEvent.Leave, @event);
        }
      }

      if (tasks.Count() > 0) {
        domain.SaveChanges();
      }
    }

    private void HandleReadyJobs(DomainContext domain, WcsDoorFactory doors)
    {

    }

    protected override Task HandleJob(CancellationToken token)
    {
      UseDomainContext(HandleRcsTasks);
      UseContext(HandleReadyJobs);

      return Task.CompletedTask;
    }
  }
}
