using DBCore;
using Midos.Center.Database;

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
