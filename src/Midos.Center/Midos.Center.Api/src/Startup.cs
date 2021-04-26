using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Midos.Domain;
using Midos.Center.Database;
using DBCore;
using Savorboard.CAP.InMemoryMessageQueue;

namespace Midos.Center
{
  public class Startup
  {
    private IConfiguration _conf;

    public Startup(IConfiguration conf)
    {
      _conf = conf;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddExceptionHandler();
      services.AddControllers();
      services.AddSingleton<AppConfig>();
      services.AddSingleton<IDomainContextOptions<DomainContext>, ServiceOptions>();
      services.AddDbContext<DomainContext, ServiceContext>();
      services.AddScoped<IMigrator, PostgresMigrator>();
      services.AddScoped<ServiceContext>();
      services.AddScoped<IRepositoryFactory, RepositoryFactory>();
      services.AddScoped<IEventPublisher, EventPublisher>();
      services.AddCap(cap => {
        cap.ConsumerThreadCount = 5;
        cap.FailedRetryCount = 0;
        cap.UseInMemoryMessageQueue();
        cap.UseInMemoryStorage();
        cap.UseDashboard();
      });
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
