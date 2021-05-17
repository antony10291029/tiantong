using Midos.SeedWork.Utils;

namespace Microsoft.Extensions.Hosting
{
  public static class IHostBuilderExtensions
  {
    public static IHostBuilder ConfigureEnvironment(this IHostBuilder builder, string env = null)
      => builder.UseEnvironment(EnvironmentUtils.GetEnvironment(env));
  }
}
