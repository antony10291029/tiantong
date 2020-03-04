using Renet.Web;
using Microsoft.Extensions.Configuration;

namespace Tiantong.Wms.Api
{
  public class AppController : BaseController
  {
    private IConfiguration _config;

    public AppController(IConfiguration config)
    {
      _config = config;
    }

    public object Home()
    {
      return JsonMessage(_config["app_name"]);
    }
  }
}
