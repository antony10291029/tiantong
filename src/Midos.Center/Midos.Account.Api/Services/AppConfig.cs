using Microsoft.Extensions.Configuration;
using Midos.App;

namespace Midos.Account.Api
{
  public class AppConfig: IAppConfig
  {
    public string AppName { get; }

    public string AppVersion { get; }

    public string AppDatabase { get; }

    public AppConfig(IConfiguration config)
    {
      AppName = config.GetValue<string>("app.name");
      AppVersion = config.AppVersion();
      AppDatabase = config.GetValue<string>("App.Database");
    }
  }
}
