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

    public new class User
    {
      [Required]
      public int? Id { get; set; }

      [Required]
      public string Name { get; set; }
    }

    public User Validate([FromBody] User user)
    {

      return user;
    }

    public void CustomerValidate()
    {
      var ex = new HttpValidationException();

      ex.AddDetails("id", "id field is required", "id must be integer");

      throw ex;
    }
  }
}
