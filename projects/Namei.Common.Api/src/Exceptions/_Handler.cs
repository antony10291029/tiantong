using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Renet;
using Renet.Web;
using Microsoft.EntityFrameworkCore;

namespace Namei.Common.Api
{
  public class ExceptionHandler : ExceptionHandlerProvider
  {
    protected override async Task Handle(Exception ex, HttpContext context)
    {
      HandleDbContext(context);

      if (ex is IKnownException) {
        await HandleKnownException((IKnownException) ex, context);
      } else if (ex is IHttpException) {
        await HandleHttpException((IHttpException) ex, context);
      } else if (Env.IsDevelopment()) {
        await ShowDevelopmentException(ex, context, ResolveExceptionExpander(ex));
      } else {
        await ShowUnprocessedError(ex, context);
      }
    }

    private void HandleDbContext(HttpContext context)
    {
      // var log = context.RequestServices.GetService<LogContext>();
      // var system = context.RequestServices.GetService<SystemContext>();

      // if (log.HasTransaction()) log.Rollback();
      // if (system.HasTransaction()) system.Rollback();
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
