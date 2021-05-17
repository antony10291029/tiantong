using AspNetCore.Proxy;
using Microsoft.Extensions.DependencyInjection;

namespace Namei.ApiGateway.Server
{
  public static class IServiceCollectionExtensions
  {
    public static void AddHttpTrackServices(this IServiceCollection services)
    {
      services.AddProxies();
      services.AddSingleton<HttpLogService>();
      services.AddScoped<HttpLogTracker>();
      services.AddHostedService<HttpLogHostedService>();
    }
  }
}
