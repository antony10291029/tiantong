using Microsoft.Extensions.Configuration;
using Midos.SeedWork.Services;

namespace Namei.ApiGateway.TestServer
{
  public class AppConfig: IAppInfo
  {
    public string AppName { get; }

    public string Server { get; }

    public string PostgresContext { get; }

    public string LoggerContext { get; }

    public AppConfig(IConfiguration conf)
    {
      AppName = conf.GetValue<string>("App.Name");
      Server = "Default";
      PostgresContext = conf.GetValue<string>("Postgres.Connection");
      LoggerContext = conf.GetValue<string>("Logger.Database.Connection");
    }
  }
}
