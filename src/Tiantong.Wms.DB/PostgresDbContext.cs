using DBCore.Postgres;
using Microsoft.EntityFrameworkCore;

namespace Tiantong.Wms.DB
{
  public class PostgresContext : DBCore.DbContext
  {
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      var builder = new PostgresBuilder();

      builder.Host("localhost").Port(5432).Database("wms").Password("123456");
      builder.OnConfiguring(options);
    }
  }
}
