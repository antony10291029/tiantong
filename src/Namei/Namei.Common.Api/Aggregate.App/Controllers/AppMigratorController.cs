using DBCore;
using Microsoft.AspNetCore.Mvc;

namespace Namei.Aggregates
{
  public class AppMigratorController: MigratorControllerBase
  {
    public AppMigratorController(IMigrator migrator): base(migrator)
    {

    }
  }
}
