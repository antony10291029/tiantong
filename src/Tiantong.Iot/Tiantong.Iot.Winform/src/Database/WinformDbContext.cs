using System.IO;
using Microsoft.EntityFrameworkCore;
using DBCore.Sqlite;

namespace Tiantong.Iot.Winforms
{
  public class WinformDbContext: DBCore.DbContext
  {
    private SqliteBuilder _builder;

    public DbSet<KeyValue> KeyValues { get; set; }

    public WinformDbContext()
    {
      _builder = new SqliteBuilder();

      if (!Directory.Exists("./DataSource")) {
        Directory.CreateDirectory("./DataSource");
      }

      this.UseFile("./DataSource/winform.db");
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
