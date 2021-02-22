using Midos;
using System;

namespace Microsoft.Extensions.Hosting
{
  public static class IHostBuilderExtensions
  {
    private static string GetValidEnvironment(string environment)
    {
      if (
        environment == null ||
        environment == Environments.Development ||
        environment == Environments.Staging ||
        environment == Environments.Production
      ) {
        return environment;
      } else {
        throw KnownException.Error($"environment is invalid: {environment}");
      }
    }

    public static IHostBuilder ConfigureEnvironment(this IHostBuilder builder, string env = null)
    {
      return builder.UseEnvironment(
        GetValidEnvironment(env)
          ?? Environment.GetEnvironmentVariable("Env")
          ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
          ?? Environments.Development
      );
    }
  }
}
