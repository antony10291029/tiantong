using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class ExceptionHandler : ExceptionHandlerProvider
  {
    protected override async Task Handle(Exception ex, HttpContext context)
    {
      Console.WriteLine($"IsDevelopment: {Env.IsDevelopment()}");

      if (ex is IHttpException) {
        await HandleHttpException((IHttpException) ex, context);
      } else if (Env.IsDevelopment()) {
        await ShowDevelopmentException(ex, context);
      } else {
        await ShowUnprocessedError(ex, context);
      }
    }
  }
}
