using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using DBCore;
using Midos.Domain;
using Midos.Services.Http;
using Namei.Aggregates;
using Savorboard.CAP.InMemoryMessageQueue;
using Namei.Common.Database;

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
      services.AddDbContext<SapContext>();
      services.AddDbContext<WmsContext>();
      services.AddDbContext<AppContext>();
      services.AddSingleton<IDomainContextOptions<AppContext>, AppContextOptions>();
      services.AddScoped<IHttpService, HttpService>();
      services.AddScoped<IEventPublisher, EventPublisher>();
      services.AddScoped<DomainContext, AppContext>();
      services.AddScoped<IMigrator, AppMigrator>();
      services.AddCap(cap => {
        cap.UseInMemoryStorage();
        cap.UseInMemoryMessageQueue();
      });
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
