using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Dynamic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
  public class ExceptionHandler
  {
    private HttpContext _httpContext;

    private IHostingEnvironment _env;

    public ExceptionHandler(
      IHostingEnvironment env,
      IHttpContextAccessor httpAccessor
    ) {
      _env = env;
      _httpContext = httpAccessor.HttpContext;
    }

    public void HandleStatusCode(int code)
    {
      if (code == 404) {
        throw new HttpException("Api not found", 404);
      }
    }

    public async Task Handle(Exception ex)
    {
      if (ex is IKnownException) {
        await HandleKnownException((IKnownException) ex, _httpContext);
      } else if (ex is IHttpException) {
        await HandleHttpException((IHttpException) ex, _httpContext);
      } else if (_env.IsDevelopment()) {
        await ShowDevelopmentException(ex, _httpContext, ResolveExceptionExpander(ex));
      } else {
        await ShowUnprocessedError(ex, _httpContext);
      }
    }

    protected Action<dynamic> ResolveExceptionExpander(Exception ex) => response =>
    {
      if (ex is DbUpdateException) {
        var error = ex as DbUpdateException;
        response.details = error.InnerException.ToString().Split('\n').Select(row => row.Trim());
      }
    };

    protected virtual async Task HandleKnownException(IKnownException ex, HttpContext context)
    {
      var httpException = new HttpException(ex.Message, ex.ErrorCode);

      await HandleHttpException(httpException, context);
    }

    protected virtual async Task HandleHttpException(IHttpException ex, HttpContext context)
    {
      context.Response.StatusCode = ex.Status;

      await context.Response.WriteAsync(ex.Body);
    }

    protected virtual async Task ShowUnprocessedError(Exception ex, HttpContext context)
    {
      context.Response.StatusCode = 500;

      await context.Response.WriteAsync(JsonSerializer.Serialize(new {
        error = "ServerException",
        message = "sorry, some exception occured.",
      }));
    }

    protected virtual async Task ShowDevelopmentException(Exception ex, HttpContext context, Action<dynamic> expander = null)
    {
      dynamic data = new ExpandoObject();
      context.Response.StatusCode = 500;

      data.error = ex.GetType().Name;
      data.message = ex.Message;
      if (expander != null) expander(data);
      data.traces = ex.StackTrace.Split("\n").Select(text => text.Trim()).ToArray();

      await context.Response.WriteAsync(JsonSerializer.Serialize((object) data));
    }
  }
}
