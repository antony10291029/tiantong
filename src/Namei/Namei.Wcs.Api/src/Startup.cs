using DBCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Midos.Domain;
using Midos.Eventing;
using Midos.Utils;
using Namei.Wcs.Aggregates;
using Namei.Wcs.Database;
using Tiantong.Iot.Utils;

namespace Namei.Wcs.Api
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddExceptionHandler();
      services.AddControllers();
      services.AddEventing(eventing => eventing.UseInMemoryQueue());
      services.AddHttpClient();
      services.AddHttpContextAccessor();
      services.AddHostedService<DoorTaskHostedService>();

      services.AddRcsServices();
      services.AddLifterServices();

      services.AddTransient<PlcStateService>();

      services.AddSingleton<Config>();
      services.AddSingleton<IAppConfig, Config>();
      services.AddSingleton<IDomainContextOptions<DomainContext>, DomainOptions>();
      services.AddSingleton<PlcStateServiceProvider>();
      services.AddSingleton<FirstLifterService>();
      services.AddSingleton<SecondLifterService>();
      services.AddSingleton<ThirdLifterService>();
      services.AddSingleton<ILifterServiceFactory, LifterServiceManager>();
      services.AddSingleton<IWmsService, WmsService>();
      services.AddSingleton<IRandom, Random>();

      services.AddDbContext<DomainContext>(); // todo remove
      services.AddDbContext<WcsContext>();
      services.AddDbContext<Midos.Domain.DomainContext, DomainContext>();

      services.AddScoped<Midos.Domain.IEventPublisher, Midos.Domain.EventPublisher>();
      services.AddScoped<IMigrator, PostgresMigrator>();
      services.AddScoped<IAgcTaskService, AgcTaskService>();
      services.AddScoped<IWcsDoorFactory, WcsDoorFactory>();
      services.AddScoped<DeviceErrorService>();
      services.AddScoped<ILifterLogger, LifterLogger>();
      services.AddScoped<Logger>();

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
