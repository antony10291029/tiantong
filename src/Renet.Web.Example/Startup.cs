using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Renet.Web.Example
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {

    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseProvider<TestAppProvider>();
    }
  }
}
