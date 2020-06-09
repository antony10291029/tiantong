using Renet;
using System;
using System.Dynamic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Renet.Web
{
  public abstract class ExceptionHandlerProvider : AppProvider
  {
    protected IApplicationBuilder App;

    protected IWebHostEnvironment Env;

    public override void Configure(IApplicationBuilder app)
    {
      Env = app.ApplicationServices.GetService<IWebHostEnvironment>();

      app.Use(async (context, next) => {
        try {
          await next();
          HandleStatusCode(context.Response.StatusCode);
        } catch (Exception ex) {
          await Handle(ex, context);
        }
      });
    }

    protected virtual void HandleStatusCode(int code)
    {
      if (code == 404) {
        throw new HttpException("Api not found", 404);
      }
    }

    protected abstract Task Handle(Exception ex, HttpContext context);

    protected virtual async Task HandleKnownException(IKnownException ex, HttpContext context)
    {
      var httpException = new HttpException(ex.Message, 400);

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
