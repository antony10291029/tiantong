using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Midos.Domain;
using DBCore;
using Microsoft.Extensions.Hosting;
using Midos.Eventing;

namespace Midos.Center
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddExceptionHandler();
      services.AddSwaggerGen();
      services.AddEventing(eventing => eventing.UseInMemoryQueue());
      services.AddControllers();

      services.AddSingleton<AppConfig>();
      services.AddSingleton<Domain.IEventPublisher, Domain.EventPublisher>();
      services.AddSingleton<IDomainContextOptions<DomainContext>, ServiceOptions>();
      services.AddDbContext<DomainContext, ServiceContext>();
      services.AddScoped<ServiceContext>();
      services.AddScoped<IRepositoryFactory, RepositoryFactory>();
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
      if (env.IsDevelopment()) {
        SwaggerBuilderExtensions.UseSwagger(app);
        SwaggerUIBuilderExtensions.UseSwaggerUI(app, c =>
          c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiGateway")
        );
      }

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
