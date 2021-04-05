using Microsoft.EntityFrameworkCore;
using DotNetCore.CAP;
using Namei.Wcs.Database;

namespace Namei.Wcs.Api
{
  public class DomainContext: Midos.Domain.DomainContext
  {
    private Config _config;

    public DbSet<Job> Jobs { get; set; }

    public DbSet<Log> Logs { get; set; }

    public DbSet<Device> Devices { get; set; }

    public DbSet<RcsDoorTask> RcsDoorTasks { get; set; }

    public DbSet<DeviceError> DeviceErrors { get; set; }

    public DbSet<DeviceState> DeviceStates { get; set; }

    public DbSet<LifterTask> LifterTasks { get; set; }

    public DbSet<WcsDoorPassport> WcsDoorPassports { get; set; }

    public DomainContext(Config config, ICapPublisher cap): base(cap)
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
