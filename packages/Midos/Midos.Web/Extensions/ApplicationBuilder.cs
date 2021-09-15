using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace Midos.Web
{
  public static class IApplicationBuilderExtensions
  {
    public static IApplicationBuilder UseEmbeddedServer(this IApplicationBuilder app, string directory = "/", string path = "")
    {
      var fileProvider = new ManifestEmbeddedFileProvider(Assembly.GetCallingAssembly(), directory);
      var options = new FileServerOptions();

      options.RequestPath = path;
      options.FileProvider = fileProvider;

      app.UseFileServer(options);
      app.Use(async (context, next) => {
        if (context.Request.Method == HttpMethods.Get) {
          await context.Response.SendFileAsync(fileProvider.GetFileInfo("index.html"));
        } else {
          await next.Invoke();
        }
      });

      return app;
    }
  }
}
