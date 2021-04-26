using Microsoft.EntityFrameworkCore;
using Midos.Domain;

namespace Midos.Center
{
  public class ServiceOptions: DomainContextOptions<DomainContext>
  {
    private AppConfig _config;

    public ServiceOptions(AppConfig config)
    {
      _config = config;
    }

    public override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseNpgsql(_config.Postgres);
    }
  }
}
