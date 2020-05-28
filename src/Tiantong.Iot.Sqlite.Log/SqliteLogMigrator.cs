using DBCore.Sqlite;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Sqlite.Log
{
  public class SqliteLogMigrator : Migrator
  { 
    public SqliteLogMigrator(LogContext db): base(db)
    {

    }

  }

}
