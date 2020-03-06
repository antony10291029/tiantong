using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.AddHttpContextAccessor();
      services.AddDbContext<DbContext>();
      services.AddScoped<IAuth, Auth>();
      services.AddSingleton<IHash, Hash>();
      services.AddSingleton<IRandom, Random>();
      services.AddScoped<AreaRepository>();
      services.AddScoped<UserRepository>();
      services.AddScoped<KeeperRepository>();
      services.AddScoped<ProjectRepository>();
      services.AddScoped<LocationRepository>();
      services.AddScoped<WarehouseRepository>();
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseProvider<ExceptionHandler>();
      app.UseMiddleware<JsonBody>();
      app.UseRouting();
      app.UseProvider<WebRoutes>();
    }
  }
}
