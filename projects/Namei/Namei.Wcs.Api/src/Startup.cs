using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Midos.Domain;
using Namei.Wcs.Aggregates;
using Savorboard.CAP.InMemoryMessageQueue;
using Tiantong.Iot.Utils;

namespace Namei.Wcs.Api
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddExceptionHandler();
      services.AddControllers();
      services.AddHttpClient();
      services.AddHttpContextAccessor();
      services.AddHostedService<DoorTaskHostedService>();
      services.AddSingleton<Config>();
      services.AddSingleton<IAppConfig, Config>();
      services.AddSingleton<IDomainContextOptions<DomainContext>, DomainOptions>();
      services.AddSingleton<PlcStateServiceProvider>();
      services.AddSingleton<DoorServiceManager>();
      services.AddTransient<PlcStateService>();
      services.AddSingleton<FirstLifterService>();
      services.AddSingleton<SecondLifterService>();
      services.AddSingleton<ThirdLifterService>();
      services.AddSingleton<LifterServiceManager>();
      services.AddSingleton<WmsService>();
      services.AddDbContext<DomainContext>();
      services.AddScoped<IEventPublisher, EventPublisher>();
      services.AddScoped<RcsService>();
      services.AddScoped<IRcsAgcTaskService, RcsAgcTaskService>();
      services.AddScoped<DeviceErrorService>();
      services.AddScoped<LifterLogger>();
      services.AddScoped<MigratorProvider>();
      services.AddScoped<Logger>();
      services.AddScoped<WcsDoorFactory>();
      services.AddCap(cap => {
        cap.ConsumerThreadCount = 10;
        cap.FailedRetryCount = 0;
        cap.UseInMemoryStorage();
        cap.UseInMemoryMessageQueue();
        cap.UseDashboard();
      });
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseMiddleware<JsonBody>();
      app.AddExceptionHandler();
      app.UseRouting();
      app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
      app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
  }
}
