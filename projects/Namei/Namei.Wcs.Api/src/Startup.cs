using DBCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Midos.Domain;
using Midos.Utils;
using Namei.Wcs.Aggregates;
using Namei.Wcs.Database;
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
      services.AddTransient<PlcStateService>();
      services.AddSingleton<FirstLifterService>();
      services.AddSingleton<SecondLifterService>();
      services.AddSingleton<ThirdLifterService>();
      services.AddSingleton<ILifterServiceFactory, LifterServiceManager>();
      services.AddSingleton<WmsService>();
      services.AddDbContext<DomainContext>(); // todo remove
      services.AddDbContext<WcsContext>();
      services.AddSingleton<IRandom, Random>();
      services.AddDbContext<Midos.Domain.DomainContext, DomainContext>();
      services.AddScoped<IMigrator, PostgresMigrator>();
      services.AddScoped<IEventPublisher, EventPublisher>();
      services.AddScoped<RcsService>(); // todo remove
      services.AddScoped<IRcsService, RcsService>();
      services.AddScoped<IRcsAgcTaskService, RcsAgcTaskService>();
      services.AddScoped<IWcsDoorFactory, WcsDoorFactory>();
      services.AddScoped<DeviceErrorService>();
      services.AddScoped<LifterLogger>();
      services.AddScoped<Logger>();
      services.AddCap(cap => {
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
