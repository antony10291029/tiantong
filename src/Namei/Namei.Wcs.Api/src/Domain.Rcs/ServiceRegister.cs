using Microsoft.Extensions.DependencyInjection;
using Midos.Domain;
using Namei.Wcs.Api;

namespace Namei.Wcs.Aggregates
{
  public static class RcsServicesRegister
  {
    public static void AddRcsServices(this IServiceCollection services)
    {
      services.AddSingleton<IDomainContextOptions<RcsContext>, RcsContextOptions>();
      services.AddDbContext<RcsContext>();
      services.AddScoped<IRcsService, RcsService>();
    }
  }
}
