using Microsoft.Extensions.DependencyInjection;
using Midos.Eventing;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Namei.Wcs.Api
{
  // 后台托管服务
  public class DoorTaskHostedService: IntervalService
  {
    private readonly IEventPublisher _publisher;

    private readonly IServiceProvider _services;

    public DoorTaskHostedService(
      IEventPublisher publisher,
      IServiceProvider services
    ) {
      _publisher = publisher;
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

    private void UseContext(Action<DomainContext, IWcsDoorFactory> callback)
    {
      using (var scope = _services.CreateScope())
      {
        var domain = scope.ServiceProvider.GetService<DomainContext>();
        var doors = scope.ServiceProvider.GetService<IWcsDoorFactory>();

        callback(domain, doors);
      }
    }

    private void HandleRcsTasks(DomainContext domain)
    {
      var expiredAt = DateTime.Now.AddSeconds(-20);

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
          _publisher.Publish(RcsDoorEvent.Retry, @event);
        } else {
          _publisher.Publish(RcsDoorEvent.Leave, @event);
        }
      }

      if (tasks.Length > 0) {
        domain.SaveChanges();
      }
    }

    private void HandleReadyJobs(DomainContext domain, IWcsDoorFactory doors)
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
