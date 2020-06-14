using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Renet.Web;

namespace Yuchuan.IErp.Proxy.Chanjet
{
  public class ExceptionHandler : ExceptionHandlerProvider
  {
    protected override async Task Handle(Exception ex, HttpContext context)
    {
      await Task.CompletedTask;
    }
  }
}
