using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Renet.Web
{
  public class JsonBody
  {
    private RequestDelegate _next;

    public JsonBody(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      context.Response.OnStarting(state => {
        var _context = state as HttpContext;

        if (_context.Request.Method == HttpMethods.Post) {
          _context.Response.ContentType = "application/json";
        }

        return Task.CompletedTask;
      }, context);

      await _next(context);
    }
  }
}
