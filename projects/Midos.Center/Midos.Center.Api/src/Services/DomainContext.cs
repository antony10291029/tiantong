using Microsoft.EntityFrameworkCore;
using Midos.Center.Database;

namespace Midos.Center
{
  public class DomainContext: PostgresContext
  {
    private AppConfig _config;

    public DomainContext(AppConfig config)
    {
      _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseNpgsql(_config.Postgres);
    }
  }
}
