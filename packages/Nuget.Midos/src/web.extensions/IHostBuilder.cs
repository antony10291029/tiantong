using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace Midos.Extensions
{
  public static class IHostBuilderExtensions
  {
    public static IHostBuilder ConfigureEnvironment(this IHostBuilder builder, string environment = null)
    {
      var env = environment
        ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
        ?? Environment.GetEnvironmentVariable("Env");

      if (
        env != Environments.Development &&
        env != Environments.Production &&
        env != Environments.Staging
      ) {
        env = Environments.Development;
      }

      return builder.UseEnvironment(env);
    }
  }
}
