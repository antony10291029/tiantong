using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Midos.Center
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.AddSingleton<AppConfig>();
      services.AddDbContext<DomainContext>();
      services.AddScoped<MigratorProvider>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseRouting();
      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
      });
    }
  }
}
