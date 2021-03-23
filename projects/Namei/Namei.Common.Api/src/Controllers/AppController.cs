using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Namei.Common.Api
{
  public class AppController: BaseController
  {
    private Config _config;

    private WmsContext _wms;

    public AppController(Config config, WmsContext wms)
    {
      _config = config;
      _wms = wms;
    }

    [Route("/")]
    public ActionResult<object> Home()
    {
      return new {
        message = $"{_config.AppName} v{_config.AppVersion}",
        env = _config.Env,
        data = _wms.AsnDetails
          .Include(ad => ad.Asn)
          .Include(ad => ad.Item)
          .Take(100)
          .ToArray()
      };
    }
  }
}
