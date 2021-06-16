using Microsoft.Extensions.DependencyInjection;

namespace Namei.Wcs.Aggregates
{
  public static class LifterServices
  {
    public static void AddLifterServices(this IServiceCollection services)
    {
      services.AddScoped<FirstLifterCommand>();
      services.AddScoped<SecondLifterCommand>();
      services.AddScoped<ThirdLifterCommand>();
      services.AddScoped<ILifterService, LifterService>();
      services.AddScoped<ILifterTaskRepository, LifterTaskRepository>();
      services.AddScoped<ILifterCommandService, LifterCommandService>();
      services.AddScoped<ILifterTaskSourceService, LifterTaskSourceService>();
    }
  }
}
