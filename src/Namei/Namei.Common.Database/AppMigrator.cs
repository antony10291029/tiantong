using DBCore.Postgres;
using Midos.Domain;

namespace Namei.Common.Database
{
  public class AppMigrator: Migrator
  {
    public AppMigrator(DomainContext context): base(context)
    {

    }
  }
}
