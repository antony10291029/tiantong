using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Midos.SeedWork.Services;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class SwaggerExtensions
  {
    public static void AddSwagger(this IServiceCollection services)
    {
      services.AddSwaggerGen();
    }

    public static void UseSwagger(this IApplicationBuilder app)
    {
      var env = app.ApplicationServices.GetService<IHostEnvironment>();

      if (env.IsDevelopment()) {
        var appName = app.ApplicationServices.GetService<IAppInfo>().AppName;

        SwaggerBuilderExtensions.UseSwagger(app);
        SwaggerUIBuilderExtensions.UseSwaggerUI(app, c =>
          c.SwaggerEndpoint("/swagger/v1/swagger.json", appName)
        );
      }
    }
  }
}
