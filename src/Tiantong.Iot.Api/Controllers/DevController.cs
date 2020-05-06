using Microsoft.AspNetCore.Mvc;
using DBCore;
using Renet.Web;

namespace Tiantong.Iot.Api
{
  [Route("/dev")]
  public class DevController: BaseController
  {
    private IMigrator _mg;

    public DevController(IMigrator mg)
    {
      _mg = mg;
    }

    [HttpPost]
    [Route("migrate")]
    public object Migrate()
    {
      _mg.Migrate();

      return SuccessOperation("数据库已迁移");
    }

    [HttpPost]
    [Route("rollback")]
    public object Rollback()
    {
      _mg.Rollback();

      return SuccessOperation("数据库已回档");
    }

    [HttpPost]
    [Route("refresh")]
    public object Refresh()
    {
      _mg.Refresh();

      return SuccessOperation("数据库已刷新");
    }
  }
}
