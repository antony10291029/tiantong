using Microsoft.AspNetCore.Mvc;

namespace Renet.Web
{
  [ValidateModel]
  public class BaseController : ControllerBase
  {
    public object JsonMessage(string message)
    {
      return new { message };
    }
  }
}
