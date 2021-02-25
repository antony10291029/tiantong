using DBCore.Postgres;

namespace Namei.Wcs.Database
{
  public class PostgresMigrator: Migrator
  {
    public PostgresMigrator(PostgresContext context): base(context)
    {

    }
  }
}
