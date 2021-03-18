using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
      services.AddDbContext<DomainContext>();
      services.AddScoped<MigratorProvider>();
      services.AddCap(cap => {
        cap.UsePostgreSql(p => {
          p.ConnectionString = _conf.GetValue<string>("postgres");
          p.Schema = _conf.GetValue<string>("cap.postgres.schema");
        });

        cap.UseRabbitMQ(mq => {
          mq.ExchangeName = "midos.cap";
          mq.VirtualHost = "midos.cap";
          mq.HostName = _conf.GetValue<string>("cap.rabbitmq:host");
          mq.Port = _conf.GetValue<int>("cap.rabbitmq:port");
          mq.UserName = _conf.GetValue<string>("cap.rabbitmq:username");
          mq.Password = _conf.GetValue<string>("cap.rabbitmq:password");
        });

        cap.UseDashboard();
        cap.FailedRetryCount = 0;
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
