using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Midos.Account.Api
{
  public class Program
  {
    public static void Main(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
        .UseAppSettings()
        .UseAppSwagger()
        .ConfigureServices(ConfigureServices)
        .Configure(Configure)
        .Build()
        .Run();

    public static void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();

      services.UseAppExceptionHandler();
      services.UseAppControllers();
      services.UseAppConfig<AppConfig>();
      services.UseAppDbContextSeeder<AppDbContextSeeder>();
      services.UseAppDbContext<AppContext, AppContextOptions>();

      services.AddScoped<UserDomainService>();
    }

    public static void Configure(IApplicationBuilder app)
    {
      app.UseAppExceptionMiddleware();
      app.UseAppSwaggerDashboard();

      app.UseRouting();
      app.UseEndpoints(endpoints => {
        endpoints.MapNotFound();
        endpoints.MapControllers();
      });
    }
  }
}
