using DBCore;
using Renet.Web;
using Microsoft.AspNetCore.Mvc;

namespace Namei.Wcs.Api
{
  public class DevController: BaseController
  {
    private IMigrator _mg;

    public DevController(MigratorProvider mg)
    {
      _mg = mg.Migrator;
    }

    [Route("/dev/migrate")]
    public object Migrate()
    {
      _mg.Migrate();

      return new {
        message = "数据库已迁移"
      };
    }

    [Route("/dev/refresh")]
    public object Refresh()
    {
      _mg.Refresh();

      return new {
        message = "数据库已刷新"
      };
    }
  }
}
