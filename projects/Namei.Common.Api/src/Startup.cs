using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Namei.Common.Api
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddExceptionHandler();
      services.AddControllers();
      services.AddHttpClient();
      services.AddSingleton<Config>();
      services.AddDbContext<RcsContext>();
      services.AddSingleton<RcsHttpService>();
    }

    public void Configure(IApplicationBuilder app)
    {
      app.AddExceptionHandler();
      app.UseMiddleware<JsonBody>();
      app.UseRouting();
      app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
      app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
  }
}
