using DBCore.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Wcs.Plc.DB.Sqlite
{
  public class SqliteDbContext : Wcs.Plc.Database.DbContext
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
