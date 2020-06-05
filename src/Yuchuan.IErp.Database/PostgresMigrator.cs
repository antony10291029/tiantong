using DBCore.Postgres;

namespace Yuchuan.IErp.Database
{
  public class PostgresMigrator : Migrator
  {
    public PostgresMigrator(PostgresContext db): base(db)
    {

    }
  }
}
