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
    [Route("/development/migrate")]
    public object Migrate()
    {
      _migrator.Migrate();

      return SuccessOperation("数据已迁移");
    }

    [HttpPost]
    [Route("/development/rollback")]
    public object Rollback()
    {
      _migrator.Rollback();

      return SuccessOperation("数据已回滚");
    }

    [HttpPost]
    [Route("/development/refresh")]
    public object Refresh()
    {
      _migrator.Refresh();

      return SuccessOperation("数据库已刷新");
    }
  }
}
