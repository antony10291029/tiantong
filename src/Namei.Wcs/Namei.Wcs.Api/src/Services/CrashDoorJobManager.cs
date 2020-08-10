using DotNetCore.CAP;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Namei.Wcs.Api
{
  public class CrashDoorJobManager: IntervalService
  {
    private IServiceScopeFactory _scopeFactory;

    public CrashDoorJobManager(IServiceScopeFactory scopeFactory)
    {
      Time = 5000;
      _scopeFactory = scopeFactory;
      UseScope((domain, _) => EnsureJobsCreated(domain));
    }

    private void EnsureJobsCreated(DomainContext domain)
    {
      foreach (var id in CrashDoor.Enumerate()) {
        if (!domain.Jobs.Any(job => job.name == $"防撞门 - {id}")) {
          domain.Add(new Job {
            name = $"防撞门 - {id}",
            interval  = 10000,
            is_enable = true,
            count = 0,
            executed_at = DateTime.MinValue,
          });
          domain.SaveChanges();
        }
      }
    }

    private void UseScope(Action<DomainContext, ICapPublisher> handler)
    {
      using (var scope = _scopeFactory.CreateScope()) {
        var domain = scope.ServiceProvider.GetService<DomainContext>();
        var cap = scope.ServiceProvider.GetService<ICapPublisher>();
        handler(domain, cap);
      }
    }

    protected override Task HandleJob(CancellationToken token)
    {
      UseScope((domain, cap) => {
        foreach  (var id in CrashDoor.Enumerate()) {
          var door = domain.Jobs.First(job => job.name == $"防撞门 - {id}");

          if (door.is_enable && door.executed_at.AddSeconds(10) < DateTime.Now) {
            cap.Publish(DoorRequestedCloseEvent.Message, new DoorClosedEvent(id));
          }
        }
      });

      return Task.CompletedTask;
    }
  }
}
