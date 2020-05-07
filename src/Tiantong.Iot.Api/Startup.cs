using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using DBCore;
using Renet;
using Renet.Web;
using Tiantong.Iot.Entities;
using Tiantong.Iot.DB.Sqlite;

namespace Tiantong.Iot.Api
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.AddSingleton<Config>();
      services.AddSingleton<IRandom, Random>();
      services.AddDbContext<IotDbContext, IotSqliteDbcontext>();
      services.AddScoped<IMigrator, IotSqliteMigrator>();
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseMiddleware<JsonBody>();
      app.UseProvider<ExceptionHandler>();
      app.UseRouting();
      app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
  }
}
