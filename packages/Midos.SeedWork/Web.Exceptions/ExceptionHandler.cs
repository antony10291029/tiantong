using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Dynamic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.Mime;

namespace Microsoft.AspNetCore.Builder
{
  public class ExceptionHandler
  {
    private readonly HttpContext _httpContext;

    private readonly IWebHostEnvironment _env;

    private readonly ILogger<ExceptionHandler> _logger;

    public ExceptionHandler(
      IWebHostEnvironment env,
      IHttpContextAccessor httpAccessor,
      ILogger<ExceptionHandler> logger
    ) {
      _env = env;
      _logger = logger;
      _httpContext = httpAccessor.HttpContext;
    }

    public static void HandleStatusCode(int code)
    {

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
      _logger.LogError(ex, "Unhandled Exception");

      _httpContext.Response.ContentType = MediaTypeNames.Application.Json;

      if (ex is IHttpException httpException) {
        await HandleHttpException(httpException, _httpContext);
      } else if (ex is InvalidOperationException && ex?.InnerException?.Message == "Exception while connecting") {
        // 无法准确定位异常
        await HandleNpgsqlTimeoutException(ex, _httpContext);
      } else {
        await ShowDevelopmentException(ex, _httpContext, ResolveExceptionExpander(ex));
      }
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
