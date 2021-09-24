using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Midos.Domain;
using Namei.Aggregates;
using Midos.Eventing;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Xml;

namespace Namei.Common.Api
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddExceptionHandler();
      services.AddSwaggerGen();
      services.AddControllers(options =>
        options.OutputFormatters.Add(new XmlSerializerOutputFormatter(new XmlWriterSettings() {
          OmitXmlDeclaration = false,
        }))
      ).AddXmlSerializerFormatters();
      services.AddHttpClient();
      services.AddEventing(eventing => eventing.UseInMemoryQueue());
      services.AddSingleton<Config>();
      services.AddDbContext<RcsContext>();
      services.AddSingleton<RcsHttpService>();
      services.AddDbContext<SapContext>();
      services.AddDbContext<WmsContext>();
      services.AddDbContext<MesContext>();
      services.AddDbContext<AppContext>();
      services.AddSingleton<IDomainContextOptions<AppContext>, AppContextOptions>();
      services.AddScoped<Midos.Domain.IEventPublisher, Midos.Domain.EventPublisher>();
      services.AddScoped<DomainContext, AppContext>();
    }

    public void Configure(IApplicationBuilder app)
    {
      app.AddExceptionHandler();
      app.UseSwagger();
      app.UseSwaggerUI(c =>
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Namei.Common.Api")
      );
      app.UseRouting();
      app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
      app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
  }
}
