using Microsoft.Extensions.Configuration;
using DBCore.Postgres;

namespace Tiantong.Wms.Api
{
  public class DbBuilder: PostgresBuilder
  {
    public DbBuilder(IConfiguration conf)
    {
      Host(conf.GetValue("pg:host", "localhost"));
      Port(conf.GetValue("pg:port", 5432));
      Username(conf.GetValue("pg:username", "postgres"));
      Password(conf.GetValue("pg:password", "password"));
      Database(conf.GetValue("pg:database", "postgres"));
    }
  }  
}
