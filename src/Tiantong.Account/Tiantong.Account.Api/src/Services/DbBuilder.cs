using Microsoft.Extensions.Configuration;
using DBCore.Postgres;

namespace Tiantong.Account.Api
{
  public class DbBuilder: PostgresBuilder
  {
    public DbBuilder(Config config)
    {
      Host(config.PG_HOST);
      Port(config.PG_PORT);
      Username(config.PG_USERNAME);
      Password(config.PG_PASSWORD);
      Database(config.PG_DATABASE);
    }
  }  
}
