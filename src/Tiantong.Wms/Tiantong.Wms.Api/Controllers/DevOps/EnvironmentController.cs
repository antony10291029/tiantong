using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class EnvironmentController : BaseController
  {
    private Config _config;

    public EnvironmentController(Auth auth, Config config)
    {
      _config = config;
      auth.EnsureRoot();
    }

    [HttpPost]
    [Route("/env")]
    public object Home()
    {
      return new {
        environment = _config.ENV
      };
    }

  }
}
