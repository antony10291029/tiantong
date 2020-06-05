using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.DependencyInjection;
using Renet;
using Renet.Web;

namespace Yuchuan.IErp.Api
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.AddSingleton<Config>();
      services.AddSingleton<IHash, Hash>();
      services.AddSingleton<IRandom, Random>();
      services.AddSingleton<DbBuilder>();
      services.AddScoped<DomainContext>();
      services.AddScoped<MigratorProvider>();
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseMiddleware<JsonBody>();
      app.UseProvider<ExceptionHandler>();
      app.UseClient();
      app.UseRouting();
      app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
      app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
  }

  public static class Extensions
  {
    public static IApplicationBuilder UseClient(this IApplicationBuilder app)
    {
      var fileProvider = new ManifestEmbeddedFileProvider(typeof(Program).Assembly, "/");

      app.UseFileServer(new FileServerOptions {
        RequestPath = "",
        FileProvider = fileProvider,
        EnableDirectoryBrowsing = true,
      });

      return app;
    }

  }

}
