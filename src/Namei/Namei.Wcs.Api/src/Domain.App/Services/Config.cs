using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace Namei.Wcs.Api
{
  public interface IAppConfig
  {
    string Env { get; }

    bool IsProduction { get; }

    bool IsDevelopment { get; }

    string AppName { get; }

    string AppKey { get; }

    string AppVersion { get; }

    string Postgres { get; }

    string PlcUrl { get; }

    string ApiGatewayUrl { get; }

    string RcsContext { get; }

    string NameiCommonUrl { get; }

    string MidosLoggerContext { get; }
  }

  public class Config: IAppConfig
  {
    public string Env { get; }

    public bool IsProduction { get => Env == "Production"; }

    public bool IsDevelopment { get => Env == "Development"; }

    public string AppName { get; }

    public string AppKey { get; }

    public string AppVersion { get; }

    public string Postgres { get; }

    public string PlcUrl { get; }

    public string ApiGatewayUrl { get; }

    public string RcsContext { get; }

    public string NameiCommonUrl { get; }

    public string MidosLoggerContext { get; }

    public Config(IConfiguration config, IHostEnvironment env)
    {
      Env = env.EnvironmentName;
      AppName = config.GetValue<string>("app_name");
      AppVersion = config.GetValue<string>("app_version");
      AppKey = config.GetValue<string>("app_key");
      Postgres = config.GetValue<string>("postgres");
      PlcUrl = config.GetValue<string>("plc_url");
      ApiGatewayUrl = config.GetValue<string>("api.gateway.url");
      RcsContext = config.GetValue<string>("rcs.context");
      NameiCommonUrl = config.GetValue<string>("namei.common.url");
      MidosLoggerContext = config.GetValue<string>("midos.logger.context");
    }
  }
}
