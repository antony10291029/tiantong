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
    private readonly HttpContext _httpContext;

    private readonly IWebHostEnvironment _env;

    public ExceptionHandler(
      IWebHostEnvironment env,
      IHttpContextAccessor httpAccessor
    ) {
      _env = env;
      _httpContext = httpAccessor.HttpContext;
    }

    public static void HandleStatusCode(int code)
    {
      if (code == 404) {
        throw new HttpException("Api not found", 404);
      }
    }

    protected static Action<dynamic> ResolveExceptionExpander(Exception ex) => response =>
    {
      if (ex is DbUpdateException) {
        var error = ex as DbUpdateException;
        response.details = error.InnerException.ToString().Split('\n').Select(row => row.Trim());
      }
    };

    public async Task Handle(Exception ex)
    {
      if (ex is IKnownException knownException) {
        await HandleKnownException(knownException, _httpContext);
      } else if (ex is IHttpException httpException) {
        await HandleHttpException(httpException, _httpContext);
      } else if (ex is InvalidOperationException && ex?.InnerException?.Message == "Exception while connecting") {
        // 无法准确定位异常
        await HandleNpgsqlTimeoutException(ex, _httpContext);
      } else {
        await ShowDevelopmentException(ex, _httpContext, ResolveExceptionExpander(ex));
      }
    }

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

    protected virtual async Task HandleNpgsqlTimeoutException(Exception ex, HttpContext context)
    {
      context.Response.StatusCode = 400;

      await context.Response.WriteAsync(JsonSerializer.Serialize(new {
        error = "DatabaseTimeoutException",
        message = "数据库连接失败，请联系技术人员",
      }));
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
      expander?.Invoke(data);
      data.traces = ex.StackTrace.Split("\n").Select(text => text.Trim()).ToArray();

      await context.Response.WriteAsync(JsonSerializer.Serialize((object) data));
    }
  }
}
