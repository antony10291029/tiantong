using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Renet.Web;
using Microsoft.EntityFrameworkCore;

namespace Tiantong.Wms.Api
{
  public class ExceptionHandler : ExceptionHandlerProvider
  {
    protected override async Task Handle(Exception ex, HttpContext context)
    {
      var db = context.RequestServices.GetService<DbContext>();
      HandleDbContext(db);

      if (ex is IHttpException) {
        await HandleHttpException((IHttpException) ex, context);
      } else if (Env.IsDevelopment()) {
        await ShowDevelopmentException(ex, context, ResolveExceptionExpander(ex));
      } else {
        await ShowUnprocessedError(ex, context);
      }
    }

    private void HandleDbContext(DbContext db)
    {
      if (db.HasTransaction()) db.Rollback();
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
