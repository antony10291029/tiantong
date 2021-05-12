using Microsoft.Extensions.DependencyInjection;

namespace Namei.ApiGateway.Server
{
  public static class IServiceCollectionExtensions
  {
    public static void AddHttpLogServices(this IServiceCollection services)
    {
      services.AddSingleton<HttpLogService>();
      services.AddScoped<HttpLogTracker>();
      services.AddHostedService<HttpLogHostedService>();
    }
  }
}
