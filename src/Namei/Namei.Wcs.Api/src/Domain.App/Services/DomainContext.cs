using Microsoft.EntityFrameworkCore;
using Midos.Domain;
using Namei.Wcs.Aggregates;

namespace Namei.Wcs.Api
{
  public class WcsContext: DomainContext
  {
    public WcsContext(IDomainContextOptions<DomainContext> options, IEventPublisher publisher)
      : base(options, publisher)
    {

    }
  }

  public class DomainContext: Midos.Domain.DomainContext
  {
    public DbSet<Job> Jobs { get; set; }

    public DbSet<Log> Logs { get; set; }

    public DbSet<Device> Devices { get; set; }

    public DbSet<RcsAgcTask> RcsAgcTasks { get; set; }

    public DbSet<RcsDoorTask> RcsDoorTasks { get; set; }

    public DbSet<DeviceError> DeviceErrors { get; set; }

    public DbSet<DeviceState> DeviceStates { get; set; }

    public DbSet<LifterTask> LifterTasks { get; set; }

    public DbSet<WcsDoorPassport> WcsDoorPassports { get; set; }

    public DbSet<LifterAgcTask> LifterAgcTasks { get; set; }

    public DbSet<LifterAgcTaskType> LifterAgcTaskTypes { get; set; }

    public DomainContext(IDomainContextOptions<DomainContext> options, IEventPublisher publisher)
      : base(options, publisher)
    {

    }
  }
}
