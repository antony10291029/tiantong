using Tiantong.Iot.Entities;
using Tiantong.Iot.DB.Sqlite;

namespace Tiantong.Iot
{
  public class DatabaseProvider
  {
    public virtual DbContext Resolve()
    {
      return new SqliteDbContext();
    }

    public virtual void Migrate()
    {
      new Migrator().UseDbContext(Resolve()).Migrate();
    }
  }
}
