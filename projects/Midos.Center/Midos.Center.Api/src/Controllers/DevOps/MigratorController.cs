using DBCore;
using Microsoft.AspNetCore.Mvc;

namespace Midos.Center.Controllers
{
  public class MigratorController: MigratorControllerBase
  {
    public MigratorController(IMigrator migrator): base(migrator)
    {

    }
  }
}
