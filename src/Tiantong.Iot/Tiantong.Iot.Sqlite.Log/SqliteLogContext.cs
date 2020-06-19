using DBCore.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Sqlite.Log
{
  public class SqliteLogContext : LogContext
  {
    private SqliteBuilder _builder;

    public SqliteLogContext()
    {
      _builder = new SqliteBuilder();

      if (!Directory.Exists("./Data")) {
        Directory.CreateDirectory("./Data");
      }

      this.UseFile("./Data/log.db");
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
