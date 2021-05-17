using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace Midos.SeedWork.Utils
{
  internal static class EnvironmentUtils
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
        throw new InvalidDataException($"environment is invalid: {environment}");
      }
    }

    public static string GetEnvironment(string env = null)
      => GetValidEnvironment(env)
        ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
        ?? Environment.GetEnvironmentVariable("Environment")
        ?? Environment.GetEnvironmentVariable("Env")
        ?? Environments.Development;
  }
}
