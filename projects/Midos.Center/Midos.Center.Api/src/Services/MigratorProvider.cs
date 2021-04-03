using DBCore;
using Midos.Center.Database;
using Midos.Domain;

namespace Midos.Center
{
  public class MigratorProvider
  {
    public IMigrator Migrator { get; }

    public MigratorProvider(DomainContext context)
    {
      Migrator = new PostgresMigrator(context);
    }
  }
}
