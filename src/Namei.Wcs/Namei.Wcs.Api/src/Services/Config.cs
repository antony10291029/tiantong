using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace Namei.Wcs.Api
{
  public class Config
  {
    public readonly string Env;

    public bool IsProduction { get => Env == "Production"; }

    public bool IsDevelopment { get => Env == "Development"; }

    public readonly string AppName;

    public readonly string AppKey;

    public readonly string AppVersion;

    public readonly string PG_CONNECTION;

    public readonly string PLC_CONNECTION;

    public Config(IConfiguration config, IHostEnvironment env)
    {
      Env = env.EnvironmentName;
      AppName = config.GetValue<string>("app_name");
      AppVersion = config.GetValue<string>("app_version");
      AppKey = config.GetValue<string>("app_key");
      PG_CONNECTION = config.GetValue<string>("pg_connection");
      PLC_CONNECTION = config.GetValue<string>("plc_connection");
    }
  }
}
