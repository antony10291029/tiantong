using Microsoft.EntityFrameworkCore;
using Midos.Center.Database;
using Midos.Center.Entities;
using Midos.Center.Aggregates;

namespace Midos.Center
{
  public class ServiceContext: PostgresContext
  {
    private AppConfig _config;

    private DbSet<App> Apps { get; set; }

    private DbSet<Config> Configs { get; set; }

    private DbSet<TaskType> TaskTypes { get; set; }

    private DbSet<SubtaskType> SubtaskTypes { get; set; }

    private DbSet<TaskOrder> TaskOrders { get; set; }

    private DbSet<SubtaskOrder> SubtaskOrders { get; set; }

    public ServiceContext(AppConfig config)
    {
      _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseNpgsql(_config.Postgres);
    }
  }
}
