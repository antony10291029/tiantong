using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace Tiantong.Account.Api
{
  public class Config
  {
    public readonly string Env;

    public bool IsProduction { get => Env == "Production"; }

    public bool IsDevelopment { get => Env == "Development"; }

    public readonly string AppName;

    public readonly string AppKey;

    public readonly string AppVersion;

    public string PG_HOST { get; }

    public int PG_PORT { get; }

    public string PG_USERNAME { get; }

    public string PG_PASSWORD { get; }

    public string PG_DATABASE { get; }

    public Config(IConfiguration config, IHostEnvironment env)
    {
      Env = env.EnvironmentName;
      AppName = config.GetValue<string>("app_name");
      AppVersion = config.GetValue<string>("app_version");
      AppKey = config.GetValue<string>("app_key");

      PG_HOST = config.GetValue<string>("postgres:host");
      PG_PORT = config.GetValue<int>("postgres:port");
      PG_USERNAME = config.GetValue<string>("postgres:username");
      PG_PASSWORD = config.GetValue<string>("postgres:password");
      PG_DATABASE = config.GetValue<string>("postgres:database");
    }
  }
}
