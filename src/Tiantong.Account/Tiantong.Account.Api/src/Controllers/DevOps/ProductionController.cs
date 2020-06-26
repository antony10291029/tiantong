using DBCore;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Account.Api
{
  public class ProductionController: BaseController
  {
    private IMigrator _mg;

    public ProductionController(
      MigratorProvider provider
    ) {
      _mg =  provider.Migrator;
    }

    [Route("production/migrate")]
    public object Migrate()
    {
      _mg.Migrate();

      return new {
        message = "数据库已迁移"
      };
    }
  }
}
