using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.AspNetCore.Builder
{
  public static class IServiceCollectionExtensions
  {
    public static IServiceCollection AddExceptionHandler(this IServiceCollection services)
    {
      services.AddHttpContextAccessor();
      services.TryAddScoped<ExceptionHandler>();

      return services;
    }
  }

  public static class IApplicationBuilderExtensions
  {
    public static IApplicationBuilder AddExceptionHandler(this IApplicationBuilder app)
    {
      app.UseMiddleware<ExceptionMiddleware>();

      return app;
    }
  }
}
