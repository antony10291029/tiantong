using System.IO;
using Microsoft.EntityFrameworkCore;
using DBCore.Sqlite;

namespace Tiantong.Iot.DB.Sqlite
{
  public class SqliteDbContext : Tiantong.Iot.Entities.DbContext
  {
    private SqliteBuilder _builder;

    public SqliteDbContext()
    {
      _builder = new SqliteBuilder();

      if (!Directory.Exists("./DataSource")) {
        Directory.CreateDirectory("./DataSource");
      }

      this.UseFile("./DataSource/sqlite.db");
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
