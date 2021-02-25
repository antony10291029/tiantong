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

    public readonly string Postgres;

    public readonly string PlcUrl;

    public readonly string WmsUrl;

    public readonly string RcsUrl;

    public Config(IConfiguration config, IHostEnvironment env)
    {
      Env = env.EnvironmentName;
      AppName = config.GetValue<string>("app_name");
      AppVersion = config.GetValue<string>("app_version");
      AppKey = config.GetValue<string>("app_key");
      Postgres = config.GetValue<string>("postgres");
      PlcUrl = config.GetValue<string>("plc_url");
      WmsUrl = config.GetValue<string>("wms_url");
      RcsUrl = config.GetValue<string>("rcs_url");
    }
  }
}
