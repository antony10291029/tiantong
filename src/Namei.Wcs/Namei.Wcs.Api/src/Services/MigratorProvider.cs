using DBCore;
using Namei.Wcs.Database;

namespace Namei.Wcs.Api
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
