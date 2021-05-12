using AspNetCore.Proxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Midos;
using Midos.Domain;
using Midos.Services.Logging;
using Savorboard.CAP.InMemoryMessageQueue;

namespace Namei.ApiGateway.Server
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.AddExceptionHandler();
      services.AddHttpLogServices();
      services.AddHttpContextAccessor();
      services.AddHttpContextAccessor();
      services.AddProxies();
      services.AddSingleton<AppConfig>();
      services.AddSingleton<IAppInfo, AppConfig>();
      services.AddSingleton<ProxyTable>();
      services.AddSingleton<IDomainContextOptions<DatabaseContext>, DatabaseContextOptions>();
      services.AddScoped<IEventPublisher, EventPublisher>();
      services.AddDbContext<DatabaseContext>();
      services.UseMidosLogger(logger => {
        logger.UseDbContextOptions<LoggerContextOptions>();
      });
      services.AddCap(cap => {
        cap.FailedRetryCount = 0;
        cap.UseInMemoryStorage();
        cap.UseInMemoryMessageQueue();
        cap.UseDashboard();
      });
      services.AddSwaggerGen(c => {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Namei.ApiGateway" });
      });
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
      if (env.IsDevelopment()) {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Namei.ApiGateway"));
      }

      app.AddExceptionHandler();
      app.UseRouting();
      app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
      });
    }
  }
}
