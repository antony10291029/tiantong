using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace Microsoft.AspNetCore.Builder
{
  public static class EmbeddedFileServerExtension
  {
    public static IApplicationBuilder UseEmbeddedServer(this IApplicationBuilder app, string directory = "/", string path = "")
    {
      var fileProvider = new ManifestEmbeddedFileProvider(Assembly.GetCallingAssembly(), directory);

      app.UseFileServer(new FileServerOptions {
        RequestPath = "",
        FileProvider = fileProvider,
        EnableDirectoryBrowsing = true,
      });

      return app;
    }
  }
}
