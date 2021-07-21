using Microsoft.Extensions.Configuration;

namespace Namei.Open.Server
{
  public class AppConfig
  {
    public readonly string SapDatabase;

    public readonly string MesDatabase;

    public readonly string WmsDatabase;

    public AppConfig(IConfiguration config)
    {
      SapDatabase = config.GetValue<string>("Sap.Database");
      MesDatabase = config.GetValue<string>("Mes.Database");
      WmsDatabase = config.GetValue<string>("Wms.Database");
    }
  }
}
