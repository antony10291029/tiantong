using Microsoft.AspNetCore.Mvc;
using DBCore;

namespace Namei.Wcs.Api
{
  public class MigratorController: MigratorControllerBase
  {
    public MigratorController(IMigrator migrator): base(migrator)
    {

    }
  }
}
