using DBCore.Postgres;

namespace Midos.Center.Database
{
  public class PostgresMigrator: Migrator
  {
    public PostgresMigrator(PostgresContext context): base(context)
    {

    }
  }
}
