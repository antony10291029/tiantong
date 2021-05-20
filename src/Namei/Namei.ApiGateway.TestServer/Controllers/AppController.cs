using Microsoft.AspNetCore.Mvc;

namespace Namei.ApiGateway.TestServer
{
  public class AppController
  {
    [Route("/{**rest}")]
    public object Home(string rest, [FromBody] object data)
      => new {
        message = "success",
        path = rest,
        data,
      };
  }
}
