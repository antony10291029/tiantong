using System.IO;
using Microsoft.EntityFrameworkCore;
using DBCore.Sqlite;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Sqlite.System
{
  public class SqliteSystemContext : SystemContext
  {
    private SqliteBuilder _builder;

    public SqliteSystemContext()
    {
      _builder = new SqliteBuilder();

      if (!Directory.Exists("./Data")) {
        Directory.CreateDirectory("./Data");
      }

      this.UseFile("./Data/system.db");
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
