using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Midos.SeedWork.Services;

namespace Namei.ApiGateway.Server
{
  public class AppConfig: IAppInfo
  {
    public string ApiEnv { get; }

    public string AppName { get; }

    public string Server { get; }

    public string PostgresContext { get; }

    public string LoggerContext { get; }

    public AppConfig(IConfiguration conf)
    {
      ApiEnv = GetApiEnv();
      AppName = conf.GetValue<string>("App.Name");
      Server = "Default";
      PostgresContext = conf.GetValue<string>("Postgres.Connection");
      LoggerContext = conf.GetValue<string>("Logger.Database.Connection");
    }

    static string GetApiEnv()
    {
      var value = Environment.GetEnvironmentVariable("API_ENV");

      if (
        value == null ||
        value == Environments.Development ||
        value == Environments.Staging ||
        value == Environments.Production
      ) {
        return value;
      } else {
        throw new InvalidDataException($"environment is invalid: {value}");
      }
    }
  }
}
