using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace Tiantong.Iot.Api
{
  public class Config
  {
    public readonly string Env;

    public bool IsProduction { get => Env == "Production"; }

    public bool IsDevelopment { get => Env == "Development"; }

    public readonly string AppName;

    public readonly string AppVersion;

    public Config(IConfiguration config, IHostEnvironment env)
    {
      Env = env.EnvironmentName;
      AppName = config.GetValue("app_name", "App");
      AppVersion = config.GetValue("app_version", "0.1.0");
    }
  }

}
