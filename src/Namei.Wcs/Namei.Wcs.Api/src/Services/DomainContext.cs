using Microsoft.EntityFrameworkCore;
using Namei.Wcs.Database;

namespace Namei.Wcs.Api
{
  public class DomainContext: PostgresContext
  {
    private Config _config;

    public DbSet<Job> Jobs { get; set; }

    public DbSet<Log> Logs { get; set; }

    public DbSet<Device> Devices { get; set; }

    public DbSet<DeviceError> DeviceErrors { get; set; }

    public DbSet<DeviceState> DeviceStates { get; set; }

    public DbSet<LifterTask> LifterTasks { get; set; }

    public DomainContext(Config config)
    {
      _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      options.UseNpgsql(_config.Postgres);
    }
  }
}
