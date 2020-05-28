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

    public readonly string AppKey;

    public readonly string AppVersion;

    public readonly string STMP_HOST;

    public readonly int STMP_PORT;

    public readonly string STMP_ADDRESS;

    public readonly string STMP_PASSWORD;

    public Config(IConfiguration config, IHostEnvironment env)
    {
      Env = env.EnvironmentName;
      AppName = config.GetValue("app_name", "App");
      AppVersion = config.GetValue("app_version", "0.1.0");
      AppKey = config.GetValue("app_key", "base64:t3gsHNPoDml4Y36IdR1CnIJ5LQTFqGQ46ectLWzqsXo=");

      STMP_HOST = config.GetValue("stmp:host", "smtpdm.aliyun.com");
      STMP_PORT = config.GetValue("stmp:port", 25);
      STMP_ADDRESS = config.GetValue("stmp:address", "");
      STMP_PASSWORD = config.GetValue("stmp:password", "");
    }
  }

}
