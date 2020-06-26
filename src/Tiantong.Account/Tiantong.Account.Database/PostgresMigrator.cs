using DBCore.Postgres;

namespace Tiantong.Account.Database
{
  public class PostgresMigrator: Migrator
  {
    public PostgresMigrator(PostgresContext context): base(context)
    {

    }
  }
}
