using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
  public class ExceptionMiddleware
  {
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    // IMyScopedService is injected into Invoke
    public async Task Invoke(HttpContext httpContext, ExceptionHandler handler)
    {
      try {
        await _next(httpContext);
        handler.HandleStatusCode(httpContext.Response.StatusCode);
      } catch (Exception e) {
        await handler.Handle(e);
      }
    }
  }
}
