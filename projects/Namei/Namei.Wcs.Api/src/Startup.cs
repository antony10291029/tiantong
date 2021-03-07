using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
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
      services.AddHttpContextAccessor();
      services.AddHttpContextAccessor();
      services.AddHttpContextAccessor();
      services.AddHttpContextAccessor();
      services.AddSingleton<DoorTaskManager>();
      services.AddHostedService<DoorTaskHostedService>();
      services.AddSingleton<Config>();
      services.AddSingleton<PlcStateServiceProvider>();
      services.AddSingleton<DoorServiceManager>();
      services.AddTransient<PlcStateService>();
      services.AddSingleton<FirstLifterService>();
      services.AddSingleton<SecondLifterService>();
      services.AddSingleton<ThirdLifterService>();
      services.AddSingleton<LifterServiceManager>();
      services.AddSingleton<WmsService>();
      services.AddSingleton<RcsService>();
      services.AddDbContext<DomainContext>();
      services.AddScoped<DeviceErrorService>();
      services.AddScoped<LifterLogger>();
      services.AddScoped<MigratorProvider>();
      services.AddScoped<Logger>();
      services.AddCap(cap => {
        cap.UseInMemoryStorage();
        cap.UseInMemoryMessageQueue();
        cap.FailedRetryCount = 0;
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
