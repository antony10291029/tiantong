using DBCore;
using Tiantong.Wms.DB;

namespace Tiantong.Wms.Api
{
  public class MigratorProvider
  {
    public IMigrator Migrator;

    public MigratorProvider(DbContext db)
    {
      var migrator = new PostgresMigrator(db);
      migrator.UseDbContext(db);

      Migrator = migrator;
    }
  }
}
