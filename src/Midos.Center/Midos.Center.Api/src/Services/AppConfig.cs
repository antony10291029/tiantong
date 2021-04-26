using Microsoft.Extensions.Configuration;

namespace Midos.Center
{
  public class AppConfig
  {
    public string Postgres { get; }

    public AppConfig(IConfiguration conf)
    {
      Postgres = conf.GetValue<string>("Postgres");
    }
  }
}
