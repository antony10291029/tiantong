using System;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  public class DomainContextFactory
  {
    private readonly IServiceProvider _services;

    public DomainContextFactory(IServiceProvider services)
    {
      _services = services;
    }

    public void UseLogContext(Action<LogContext> handler)
    {
      using var scope = _services.CreateScope();
      using var context = scope.ServiceProvider.GetService<LogContext>();

      handler(context);
    }

    public void UseSystemContext(Action<SystemContext> handler)
    {
      using var scope = _services.CreateScope();
      using var context = scope.ServiceProvider.GetService<SystemContext>();

      handler(context);
    }

    public void Migrate()
    {
      UseLogContext(context => context.Database.EnsureCreated());
      UseSystemContext(context => context.Database.EnsureCreated());
    }

    public void Rollback()
    {
      UseLogContext(context => context.Database.EnsureDeleted());
      UseSystemContext(context => context.Database.EnsureDeleted());
    }

    public void Refresh()
    {
      Rollback();
      Migrate();
    }

    public void Log<T>(T log)
    {
      UseLogContext(context => {
        try {
          context.Add(log);
          context.SaveChanges();
        } catch (Exception e) {
          Console.WriteLine(e);
        }
      });
    }
  }
}
