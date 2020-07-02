using Yuchuan.IErp.Database;
using Microsoft.EntityFrameworkCore;

namespace Yuchuan.IErp.Api
{
  public class DomainContext: PostgresContext
  {
    public DbSet<Device> Devices { get; set; }

    public DbSet<DeviceState> DeviceStates { get; set; }

    public DbSet<Project> Projects { get; set; }

    public DbSet<ProjectUser> ProjectUsers { get; set; }

    public DbSet<ProjectDevice> ProjectDevices { get; set; }

    public DomainContext(DbBuilder builder): base(builder)
    {

    }
  }
}
