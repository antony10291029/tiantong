using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Midos.Center.Entities;
using Midos.Center.Aggregates;
using Midos.Center.Database;
using Midos.Domain;

namespace Midos.Center
{
  public class ServiceContext: DomainContext
  {
    private AppConfig _config;

    protected DbSet<App> Apps { get; set; }

    protected DbSet<Config> Configs { get; set; }

    protected DbSet<TaskType> TaskTypes { get; set; }

    protected DbSet<SubtaskType> SubtaskTypes { get; set; }

    protected DbSet<TaskOrder> TaskOrders { get; set; }

    protected DbSet<SubtaskOrder> SubtaskOrders { get; set; }

    public ServiceContext(AppConfig config, ICapPublisher cap): base(cap)
    {
      _config = config;
      UseAssembly(typeof(PostgresMigrator).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseNpgsql(_config.Postgres);
    }
  }
}
