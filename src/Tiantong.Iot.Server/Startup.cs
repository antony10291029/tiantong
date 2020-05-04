using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Renet.Web;

namespace Tiantong.Iot.Server
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddSingleton<PlcManager>();
      services.AddHostedService<PlcManagerHostedService>();
      services.AddControllers();
      services.AddHttpContextAccessor();
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseProvider<ExceptionHandler>();
      app.UseMiddleware<JsonBody>();
      app.UseRouting();
      app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
      app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
  }
}
