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

    public string JWT_SECRET { get; }

    public int JWT_TTL { get; }

    public int JWT_RFT { get; }

    public string PG_HOST { get; }

    public int PG_PORT { get; }

    public string PG_USERNAME { get; }

    public string PG_PASSWORD { get; }

    public string PG_DATABASE { get; }

    public readonly string STMP_HOST;

    public readonly int STMP_PORT;

    public readonly string STMP_ADDRESS;

    public readonly string STMP_PASSWORD;

    public Config(IConfiguration config, IHostEnvironment env)
    {
      Env = env.EnvironmentName;
      AppName = config.GetValue<string>("app_name");
      AppVersion = config.GetValue<string>("app_version");
      AppKey = config.GetValue<string>("app_key");

      JWT_SECRET = config.GetValue<string>("jwt:secret");
      JWT_TTL = config.GetValue<int>("jwt:ttl");
      JWT_RFT = config.GetValue<int>("jwt:rft");

      STMP_HOST = config.GetValue<string>("stmp:host");
      STMP_PORT = config.GetValue<int>("stmp:port");
      STMP_ADDRESS = config.GetValue<string>("stmp:address");
      STMP_PASSWORD = config.GetValue<string>("stmp:password");

      PG_HOST = config.GetValue<string>("postgres:host");
      PG_PORT = config.GetValue<int>("postgres:port");
      PG_USERNAME = config.GetValue<string>("postgres:username");
      PG_PASSWORD = config.GetValue<string>("postgres:password");
      PG_DATABASE = config.GetValue<string>("postgres:database");
    }
  }
}
