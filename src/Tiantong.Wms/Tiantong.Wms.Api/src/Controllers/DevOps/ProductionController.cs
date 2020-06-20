using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class ProductionController : BaseController
  {
    private MigratorProvider _migratorProvider;

    private RootRepository _rootRepository;

    public ProductionController(
      Auth auth,
      RootRepository rootRepository,
      MigratorProvider migratorProvider
    ) {
      _rootRepository = rootRepository;
      _migratorProvider = migratorProvider;
      // auth.EnsureRoot();
    }

    [HttpPost]
    [Route("/prod/migrate")]
    public object Migrate()
    {
      if (_migratorProvider.Migrator.Migrate() != 0) {
        _rootRepository.Create();
      };

      return SuccessOperation("系统更新完毕");
    }

  }
}
