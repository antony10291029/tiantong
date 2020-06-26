using DBCore.Postgres;
using Microsoft.EntityFrameworkCore;

namespace Yuchuan.IErp.Database
{
  public class PostgresContext : DBCore.DbContext
  {
    private PostgresBuilder _builder;

    public PostgresContext(PostgresBuilder builder)
    {
      _builder = builder;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      _builder.OnConfiguring(options);
    }
  }
}
