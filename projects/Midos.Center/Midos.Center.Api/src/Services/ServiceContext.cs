using Microsoft.EntityFrameworkCore;
using Midos.Center.Entities;
using Midos.Center.Aggregates;
using Midos.Center.Database;
using Midos.Domain;

namespace Midos.Center
{
  public class ServiceContext: DomainContext
  {
    protected DbSet<App> Apps { get; set; }

    protected DbSet<Config> Configs { get; set; }

    protected DbSet<TaskType> TaskTypes { get; set; }

    protected DbSet<SubtaskType> SubtaskTypes { get; set; }

    protected DbSet<TaskOrder> TaskOrders { get; set; }

    protected DbSet<SubtaskOrder> SubtaskOrders { get; set; }

    public ServiceContext(IDomainContextOptions<DomainContext> options, IEventPublisher publisher)
      : base(options, publisher)
    {
      UseAssembly(typeof(PostgresMigrator).Assembly);
    }
  }
}
