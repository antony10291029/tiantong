using Tiantong.Iot.Entities;
using Tiantong.Iot.DB.Sqlite;

namespace Tiantong.Iot
{
  public class DatabaseProvider
  {
    public virtual IotDbContext Resolve()
    {
      return new IotSqliteDbcontext();
    }

    public virtual void Migrate()
    {
      new IotSqliteMigrator(Resolve()).Migrate();
    }
  }
}
