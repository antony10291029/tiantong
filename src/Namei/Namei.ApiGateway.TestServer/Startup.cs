using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Midos.SeedWork.Services.Logging;
using Midos.SeedWork.Services;

namespace Namei.ApiGateway.TestServer
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.AddHttpContextAccessor();
      services.AddHttpClient();
      services.AddSwagger();
      services.AddExceptionHandler();
      services.AddSingleton<Random>();
      services.AddSingleton<AppConfig>();
      services.AddSingleton<IAppInfo, AppConfig>();
      services.AddEFContext<AppContext, AppContextOptions>();
      services.UseMidosLogger(logger => {
        logger.UseDbContextOptions<LoggerContextOptions>();
      });
      services.AddHostedService<TestHostedService>();
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
