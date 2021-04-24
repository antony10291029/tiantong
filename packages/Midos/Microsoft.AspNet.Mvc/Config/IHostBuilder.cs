using Midos.Utils;

namespace Microsoft.Extensions.Hosting
{
  public static class IHostBuilderExtensions
  {
    public static IHostBuilder ConfigureEnvironment(this IHostBuilder builder, string env = null)
    {
      return builder.UseEnvironment(EnvironmentUtils.GetEnvironment());
    }
  }
}
