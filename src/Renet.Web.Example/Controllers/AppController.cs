using Microsoft.AspNetCore.Mvc;

namespace Renet.Web.Example
{
  public class AppController : ControllerBase
  {
    public string Home()
    {
      return "welcome to Renet.Web.Example";
    }

    public string Post()
    {
      return "post method";
    }

    public string Users()
    {
      return "users";
    }
  }
}
