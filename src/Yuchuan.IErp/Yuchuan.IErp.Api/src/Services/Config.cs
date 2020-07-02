using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace Yuchuan.IErp.Api
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

    public string SIGNALR_REDIS { get; }

    public string CACHE_REDIS { get; }

    public string CORS_ORIGIN { get; }

    public Config(IConfiguration config, IHostEnvironment env)
    {
      Env = env.EnvironmentName;
      AppName = config.GetValue("app_name", "App");
      AppVersion = config.GetValue("app_version", "0.1.0");
      AppKey = config.GetValue("app_key", "base64:t3gsHNPoDml4Y36IdR1CnIJ5LQTFqGQ46ectLWzqsXo=");

      PG_HOST = config.GetValue("postgres:host", "localhost");
      PG_PORT = config.GetValue("postgres:port", 5432);
      PG_USERNAME = config.GetValue("postgres:username", "postgres");
      PG_PASSWORD = config.GetValue("postgres:password", "password");
      PG_DATABASE = config.GetValue("postgres:database", "postgres");

      SIGNALR_REDIS = config.GetValue<string>("signalr:redis");
      CACHE_REDIS = config.GetValue<string>("cache:redis");

      CORS_ORIGIN = config.GetValue<string>("cors:origin");
    }
  }
}
