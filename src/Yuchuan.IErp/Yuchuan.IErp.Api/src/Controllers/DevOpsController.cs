using Microsoft.AspNetCore.Mvc;
using Renet.Web;
using DBCore;

namespace Yuchuan.IErp.Api
{
  [Route("/")]
  public class DevOpsController: BaseController
  {
    public IMigrator _migrator;

    public DevOpsController(MigratorProvider mg)
    {
      _migrator = mg.Migrator;
    }

    [HttpPost]
    [Route("/dev/migrate")]
    public object Migrate()
    {
      _migrator.Migrate();

      return SuccessOperation("数据已迁移");
    }

    [HttpPost]
    [Route("/dev/refresh")]
    public object Refresh()
    {
      _migrator.Refresh();

      return SuccessOperation("数据库已刷新");
    }
  }
}