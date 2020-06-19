using DBCore;
using Yuchuan.IErp.Database;

namespace Yuchuan.IErp.Api
{
  public class MigratorProvider
  {
    public IMigrator Migrator;

    public MigratorProvider(DomainContext db)
    {
      var migrator = new PostgresMigrator(db);

      Migrator = migrator;
    }
  }
}
