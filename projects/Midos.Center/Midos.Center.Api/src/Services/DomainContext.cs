using Microsoft.EntityFrameworkCore;
using Midos.Center.Database;
using Midos.Center.Entities;

namespace Midos.Center
{
  public class DomainContext: PostgresContext
  {
    private AppConfig _config;

    public DbSet<App> Apps { get; set; }

    public DbSet<Config> Configs { get; set; }

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
