using DBCore.Postgres;
using Microsoft.EntityFrameworkCore;

namespace Tiantong.Account.Database
{
  public class PostgresContext: DBCore.DbContext
  {
    private PostgresBuilder _builder;

    public PostgresContext(PostgresBuilder builder)
    {
      _builder = builder;
      UseAssembly(typeof(PostgresContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      _builder.OnConfiguring(options);
    }
  }
}
