using Microsoft.AspNetCore.Mvc;

namespace Namei.Common.Api
{
  public class AppController: BaseController
  {
    [Route("/")]
    public object Home()
    {
      return new {
        name = "Namei.Wcs.Common"
      };
    }
  }
}
