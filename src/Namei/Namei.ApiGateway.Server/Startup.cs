using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
// using Midos;
// using Midos.Domain;
// using Midos.Services.Logging;
using Midos.SeedWork.Services;

namespace Namei.ApiGateway.Server
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.AddHttpContextAccessor();
      services.AddSwagger();
      services.AddExceptionHandler();
      services.AddHttpTrackServices();
      services.AddSingleton<AppConfig>();
      services.AddSingleton<IAppInfo, AppConfig>();
      services.AddEFContext<AppContext, AppContextOptions>();
      services.AddScoped<RouteRepository>();
      // services.AddScoped<IEventPublisher, EventPublisher>();
      // services.UseMidosLogger(logger => {
      //   logger.UseDbContextOptions<LoggerContextOptions>();
      // });
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseRouting();
      app.UseSwagger();
      app.AddExceptionHandler();
      app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
      });
    }
  }
}
