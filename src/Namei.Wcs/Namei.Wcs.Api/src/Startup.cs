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
      services.AddSingleton<Config>();
      services.AddTransient<PlcStateService>();
      services.AddDbContext<DomainContext>();
      services.AddScoped<FirstLifterService>();
      services.AddScoped<SecondLifterService>();
      services.AddScoped<ThirdLifterService>();
      services.AddScoped<LifterServiceManager>();
      services.AddScoped<MigratorProvider>();
      services.AddScoped<Logger>();
      services.AddScoped<WmsService>();
      services.AddCap(cap => {
        cap.UseInMemoryStorage();
        cap.UseInMemoryMessageQueue();
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
