using Microsoft.AspNetCore.Mvc;

namespace Namei.Common.Api
{
  public class AppController: BaseController
  {
    [HttpPost("/")]
    public object Home()
    {
      return new {
        name = "Namei.Wcs.Common"
      };
    }
  }
}
