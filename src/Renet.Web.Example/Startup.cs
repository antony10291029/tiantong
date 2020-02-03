using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Renet.Web.Example
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseRouting();
      app.UseProvider<TestAppProvider>();
      app.UseProvider<WebRoutes>();
    }
  }
}
