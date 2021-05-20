using Microsoft.EntityFrameworkCore;
using Midos.SeedWork.Domain;

namespace Namei.ApiGateway.TestServer
{
  public class AppContextOptions: EFContextOptions<AppContext>
  {
    private readonly AppConfig _appConfig;

    public AppContextOptions(AppConfig appConfig)
    {
      _appConfig = appConfig;
    }

    public override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseNpgsql(_appConfig.PostgresContext);
    }
  }
}
