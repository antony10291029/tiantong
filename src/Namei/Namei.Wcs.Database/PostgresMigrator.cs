using DBCore.Postgres;
using Midos.Domain;

namespace Namei.Wcs.Database
{
  public class PostgresMigrator: Migrator
  {
    public PostgresMigrator(DomainContext context): base(context)
    {

    }
  }
}
