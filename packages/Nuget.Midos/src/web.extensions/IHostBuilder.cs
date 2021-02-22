using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace Midos.Extensions
{
  public static class IHostBuilderExtensions
  {
    private static string GetValidEnvironment(string environment)
    {
      if (
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
