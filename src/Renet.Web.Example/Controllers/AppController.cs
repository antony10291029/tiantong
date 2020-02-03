using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Renet.Web.Example
{
  public class AppController : BaseController
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

    public string Error()
    {
      throw new HttpExampleException();
    }

    public void UnexpectedError()
    {
      var a = 0;
      var b = 0;

      a = a / b;
    }
  }
}
