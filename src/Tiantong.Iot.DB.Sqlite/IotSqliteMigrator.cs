using Tiantong.Iot.Entities;

namespace Tiantong.Iot.DB.Sqlite
{
  public class IotSqliteMigrator : DBCore.Sqlite.Migrator
  { 
    public IotSqliteMigrator(IotDbContext db)
    {
      UseDbContext(db);
    }
  }
}
