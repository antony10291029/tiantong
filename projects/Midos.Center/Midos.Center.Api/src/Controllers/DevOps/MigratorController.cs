using DBCore;
using Microsoft.AspNetCore.Mvc;

namespace Midos.Center.Controllers
{
  public class MigratorController: MigratorControllerBase
  {
    protected override IMigrator _migrator { get; set; }

    public MigratorController(MigratorProvider _migratorProvider)
    {
      _migrator = _migratorProvider.Migrator;
    }
  }
}
