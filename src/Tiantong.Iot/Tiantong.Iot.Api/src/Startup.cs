using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Embedded;
using Microsoft.Extensions.Hosting;
using Midos.Utils;
using Midos.Web;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddExceptionHandler();
      services.AddControllers();
      services.AddHostedService<PlcManagerService>();
      services.AddSingleton<Mail>();
      services.AddSingleton<Config>();
      services.AddSingleton<PlcManager>();
      services.AddSingleton<IHash>(app => new Hash(app.GetService<Config>().AppKey));
      services.AddSingleton<IRandom, Random>();
      services.AddSingleton<HttpPusherClient>();
      services.AddSingleton<DomainContextFactory>();
      services.AddDbContext<LogContext, AppLogContext>();
      services.AddDbContext<SystemContext, AppSystemContext>();
      services.AddScoped<HttpPusherRepository>();
      services.AddScoped<PlcRepository>();
      services.AddScoped<PlcLogRepository>();
      services.AddScoped<PlcStateRepository>();
      services.AddScoped<PlcStateLogRepository>();
      services.AddScoped<PlcStateErrorRepository>();
      services.AddScoped<PlcStateHttpPusherRepository>();
      services.AddScoped<SystemRepository>();
      services.AddTransient<PlcBuilder>();
      services.AddSwaggerGen(options => {
        options.CustomSchemaIds(type => type.ToString());
      });

      services.AddDbContext<AppContext>();
      services.AddScoped<PlcConfigContext>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment()) {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
          c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tiantong.Iot.Api")
        );
      } else {
        app.UseEmbeddedServer();
      }

      app.AddExceptionHandler();
      app.UseRouting();
      app.UseCors(policy => policy.SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
      });
    }
  }
}