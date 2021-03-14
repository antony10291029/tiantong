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

    public DbSet<TaskType> TaskTypes { get; set; }

    public DbSet<SubtaskType> SubtaskTypes { get; set; }

    public DbSet<TaskOrder> TaskOrders { get; set; }

    public DbSet<SubtaskOrder> SubtaskOrders { get; set; }

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
