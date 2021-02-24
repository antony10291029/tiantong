using DBCore;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Namei.Wcs.Api
{
  public class DevController: BaseController
  {
    private IMigrator _mg;

    public DevController(MigratorProvider mg, Config config)
    {
      if (!config.IsDevelopment) {
        throw KnownException.Error("该接口只面向开发环境");
      }
      _mg = mg.Migrator;
    }

    [Route("/dev/migrate")]
    public object Migrate()
    {
      _mg.Migrate();

      return NotifyResult.FromVoid().Success("数据库已迁移");
    }

    [Route("/dev/rollback")]
    public object Rollback()
    {
      _mg.Rollback();

      return NotifyResult.FromVoid().Success("数据库已迁移");
    }

    [Route("/dev/refresh")]
    public object Refresh()
    {
      _mg.Refresh();

      return NotifyResult.FromVoid().Success("数据库已刷新");
    }
  }
}
