using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace Namei.Common.Api
{
  public class Config
  {
    public readonly string Env;

    public readonly string AppName;

    public readonly string AppVersion;

    public readonly string RcsDbConfig;

    public readonly string RcsUrl;

    public readonly string SapUrl;

    public readonly string WmsUrl;

    public readonly string WcsDb;

    public bool IsProduction { get => Env == "Production"; }

    public bool IsDevelopment { get => Env == "Development"; }

    public Config(IConfiguration config, IHostEnvironment env)
    {
      Env = env.EnvironmentName;
      AppName = config.GetValue<string>("app_name");
      AppVersion = config.GetValue<string>("app_version");
      RcsDbConfig = config.GetValue<string>("rcs.db.config");
      RcsUrl = config.GetValue<string>("rcs.url");
      SapUrl = config.GetValue<string>("sap.url");
      WmsUrl = config.GetValue<string>("wms.url");
      WcsDb = config.GetValue<string>("wcs.db");
    }
  }
}
