using Midos.SeedWork.Services.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting
{
  public static class IHostBuilderMidosLoggingExtensions
  {
    public static IHostBuilder UseMidosLogging(this IHostBuilder builder)
    {
      var provider = new MidosLoggerProvider();

      return builder
        .ConfigureLogging(logging => logging.AddProvider(provider))
        .ConfigureServices(services => services.AddSingleton(provider));
    }
  }
}
