using DBCore;
using Microsoft.AspNetCore.Mvc;
using Renet;
using Renet.Web;

namespace Tiantong.Account.Api
{
  public class DevelopmentController: BaseController
  {
    private IMigrator _mg;

    public DevelopmentController(
      Config config,
      MigratorProvider provider
    ) {
      if (!config.IsDevelopment) {
        throw KnownException.Error("该接口只允许在开发环境下使用");
      }

      _mg = provider.Migrator;
    }

    [HttpPost]
    [Route("/development/migrate")]
    public ActionResult<object> Migrate()
    {
      _mg.Migrate();

      return new {
        message = "数据库已迁移"
      };
    }

    [HttpPost]
    [Route("/development/rollback")]
    public ActionResult<object> Rollback()
    {
      _mg.Rollback();

      return new {
        message = "数据库已回档"
      };
    }

    [HttpPost]
    [Route("/development/refresh")]
    public ActionResult<object> Refresh()
    {
      _mg.Refresh();

      return new {
        message = "数据库已刷新"
      };
    }
  }
}
