using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Midos.Domain;
using System;

namespace Midos.Services.Logging
{
  public class LoggerBuilder
  {
    private readonly IServiceCollection _services;

    public LoggerBuilder(IServiceCollection services)
    {
      _services = services;
    }

    public void UseDbContextOptions<TOptions>()
      where TOptions: class, IDomainContextOptions<LoggerContext>
    {
      _services.TryAddSingleton<IDomainContextOptions<LoggerContext>, TOptions>();
    }
  }

  public static class IServiceCollectionExtensions
  {
    public static void UseMidosLogger(this IServiceCollection services, Action<LoggerBuilder> hook = null)
    {
      hook(new(services));
      services.AddDbContext<LoggerContext>();
      services.AddHostedService<LoggerHostedService>();
    }
  }
}
