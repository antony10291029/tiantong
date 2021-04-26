using DBCore.Postgres;
using Midos.Domain;

namespace Midos.Center.Database
{
  public class PostgresMigrator: Migrator
  {
    public PostgresMigrator(DomainContext context): base(context)
    {

    }
  }
}
