using Microsoft.EntityFrameworkCore;
using Midos.Domain;

namespace Namei.ApiGateway.Server
{
  public class DatabaseContextOptions: DomainContextOptions<DatabaseContext>
  {
    private readonly AppConfig _appConfig;

    public DatabaseContextOptions(AppConfig appConfig)
    {
      _appConfig = appConfig;
    }

    public override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseNpgsql(_appConfig.PostgresContext);
    }
  }
}
