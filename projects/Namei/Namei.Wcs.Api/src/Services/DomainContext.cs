using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Namei.Wcs.Database;
using Namei.Wcs.Aggregates;

namespace Namei.Wcs.Api
{
  public class DomainContext: Midos.Domain.DomainContext
  {
    private IAppConfig _config;

    public DbSet<Job> Jobs { get; set; }

    public DbSet<Log> Logs { get; set; }

    public DbSet<Device> Devices { get; set; }

    public DbSet<RcsAgcTask> RcsAgcTasks { get; set; }

    public DbSet<RcsDoorTask> RcsDoorTasks { get; set; }

    public DbSet<DeviceError> DeviceErrors { get; set; }

    public DbSet<DeviceState> DeviceStates { get; set; }

    public DbSet<LifterTask> LifterTasks { get; set; }

    public DbSet<WcsDoorPassport> WcsDoorPassports { get; set; }

    public DomainContext(IAppConfig config, ICapPublisher cap): base(cap)
    {
      _config = config;
      UseAssembly(typeof(PostgresMigrator).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      options.UseNpgsql(_config.Postgres);
    }
  }
}
