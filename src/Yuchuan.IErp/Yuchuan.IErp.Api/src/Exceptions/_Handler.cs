using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Renet;
using Renet.Web;
using Microsoft.EntityFrameworkCore;

namespace Yuchuan.IErp.Api
{
  public class ExceptionHandler : ExceptionHandlerProvider
  {
    protected override async Task Handle(Exception ex, HttpContext context)
    {
      if (ex is KnownException) {
        await HandleKnownException(ex as KnownException, context);
      } else if (ex is IHttpException) {
        await HandleHttpException((IHttpException) ex, context);
      } else if (Env.IsDevelopment()) {
        await ShowDevelopmentException(ex, context, ResolveExceptionExpander(ex));
      } else {
        await ShowUnprocessedError(ex, context);
      }
    }

    private Action<dynamic> ResolveExceptionExpander(Exception ex) => response =>
    {
      if (ex is DbUpdateException) {
        var error = ex as DbUpdateException;
        response.details = error.InnerException.ToString().Split('\n').Select(row => row.Trim());
      }
    };
  }
}
