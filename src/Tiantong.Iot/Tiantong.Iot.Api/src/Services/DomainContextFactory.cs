using Tiantong.Iot.Entities;
using Tiantong.Iot.Sqlite.Log;
using Tiantong.Iot.Sqlite.System;

namespace Tiantong.Iot.Api
{
  public class DomainContextFactory
  {
    public LogContext LogContext()
    {
      return new SqliteLogContext();
    }

    public SystemContext SystemContext()
    {
      return new SqliteSystemContext();
    }

    public void Migrate()
    {
      using (var db = LogContext())
      {
        var mg = new SqliteLogMigrator(db);
        mg.Migrate();
      }

      using (var db = SystemContext())
      {
        var mg = new SqliteSystemMigrator(db);
        mg.Migrate();
      }
    }

    public void Rollback()
    {
      using (var db = LogContext())
      {
        var mg = new SqliteLogMigrator(db);
        mg.Rollback();
      }

      using (var db = SystemContext())
      {
        var mg = new SqliteSystemMigrator(db);
        mg.Rollback();
      }
    }

    public void Refresh()
    {
      using (var db = LogContext())
      {
        var mg = new SqliteLogMigrator(db);
        mg.Refresh();
      }

      using (var db = SystemContext())
      {
        var mg = new SqliteSystemMigrator(db);
        mg.Refresh();
      }
    }

    public void Log<T>(T log)
    {
      using (var db = LogContext()) {
        db.Add(log);
        db.SaveChanges();
      }
    }

  }

}
