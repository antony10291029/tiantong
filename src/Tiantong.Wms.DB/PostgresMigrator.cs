using DBCore.Postgres;

namespace Tiantong.Wms.DB
{
  public class PostgresMigrator : Migrator
  {
    public PostgresMigrator(PostgresContext db): base(db)
    {

    }
  }
}
