using Microsoft.Extensions.Hosting;
using System;

namespace Midos.Utils
{
  public static class EnvironmentUtils
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

    public static string GetEnvironment(string env = null)
    {
      return GetValidEnvironment(env)
        ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
        ?? Environment.GetEnvironmentVariable("Environment")
        ?? Environment.GetEnvironmentVariable("Env")
        ?? Environments.Development;
    }
  }
}
