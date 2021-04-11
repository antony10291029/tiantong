using DotNetCore.CAP;
using Savorboard.CAP.InMemoryMessageQueue;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Namei.Wcs.Api.Test
{

  public class DomainContext: Namei.Wcs.Api.DomainContext
  {
    public DomainContext(Config config, ICapPublisher cap): base(config, cap)
    {

    }
  }

  public class AppServiceProvider
  {
    private IServiceProvider _services;

    public AppServiceProvider()
    {
      var container = new ServiceCollection();

      container.AddCap(cap => {
        cap.UseInMemoryStorage();
        cap.UseInMemoryMessageQueue();
      });

      container.AddDbContext<DomainContext>();
    }
  }
}
