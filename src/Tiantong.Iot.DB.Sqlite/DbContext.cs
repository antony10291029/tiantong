using DBCore.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Tiantong.Iot.DB.Sqlite
{
  public class SqliteDbContext : Tiantong.Iot.Database.DbContext
  {
    private SqliteBuilder _builder;

    public SqliteDbContext()
    {
      _builder = new SqliteBuilder();

      _builder.UseDbFile("./sqlite.db");
    }

    public void UseFile(string path)
    {
      _builder.UseDbFile(path);
    }

    public void UseInMemory()
    {
      _builder.UseInMemory(this);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      _builder.OnConfiguring(options);
    }

  }
}
