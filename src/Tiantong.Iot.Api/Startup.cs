using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.DependencyInjection;
using DBCore;
using Renet;
using Renet.Web;
using Tiantong.Iot.Entities;
using Tiantong.Iot.DB.Sqlite;

namespace Tiantong.Iot.Api
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.AddHostedService<PlcManagerService>();
      services.AddSingleton<Mail>();
      services.AddSingleton<Config>();
      services.AddSingleton<PlcManager>();
      services.AddSingleton<IHash, Hash>();
      services.AddSingleton<IRandom, Random>();
      services.AddSingleton<DatabaseFactory>();
      services.AddSingleton<HttpPusherClient>();
      services.AddSingleton<HttpPusherFactory>();
      services.AddDbContext<IotDbContext, IotSqliteDbcontext>();
      services.AddScoped<IMigrator, IotSqliteMigrator>();
      services.AddScoped<HttpPusherRepository>();
      services.AddScoped<PlcRepository>();
      services.AddScoped<PlcLogRepository>();
      services.AddScoped<PlcStateRepository>();
      services.AddScoped<PlcStateLogRepository>();
      services.AddScoped<PlcStateErrorRepository>();
      services.AddScoped<PlcStateHttpPusherRepository>();
      services.AddScoped<SystemRepository>();
      services.AddTransient<PlcBuilder>();
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
