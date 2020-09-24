using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Renet.Web;
using Savorboard.CAP.InMemoryMessageQueue;
using Tiantong.Iot.Utils;

namespace Namei.Wcs.Api
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.AddHttpClient();
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
      services.AddSingleton<LifterTaskService>();
      services.AddDbContext<DomainContext>();
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
      app.UseProvider<ExceptionHandler>();
      app.UseRouting();
      app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
      app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
  }
}
