using Midos.Web.Config;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.Configuration
{
  public static class IWebHostBuilderExtensions
  {
    public static IHostBuilder ConfigureMidosCenter(this IHostBuilder builder)
    {
      return builder.ConfigureAppConfiguration((_, config) => {
        config.Add(new MidosConfigSource());
      });
    }
  }
}
