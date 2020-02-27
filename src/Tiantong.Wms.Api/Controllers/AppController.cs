using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;
using Tiantong.Wms.DB;

namespace Tiantong.Wms.Api
{
  public class AppController : _BaseController
  {
    private IAuth _auth;

    public AppController(IAuth auth)
    {
      _auth = auth;
    }

    public object Home()
    {
      _auth.Ensure();

      return _auth.User;
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
