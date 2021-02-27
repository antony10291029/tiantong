using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Midos.Center
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddExceptionHandler();
      services.AddControllers();
      services.AddSingleton<AppConfig>();
      services.AddDbContext<DomainContext>();
      services.AddScoped<MigratorProvider>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.AddExceptionHandler();
      app.UseRouting();
      app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
      app.UseMiddleware<JsonBody>();
      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
      });
    }
  }
}
