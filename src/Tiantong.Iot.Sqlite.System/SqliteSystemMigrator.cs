using DBCore.Sqlite;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Sqlite.System
{
  public class SqliteSystemMigrator : Migrator
  { 
    public SqliteSystemMigrator(SystemContext db): base(db)
    {

    }

  }

}
