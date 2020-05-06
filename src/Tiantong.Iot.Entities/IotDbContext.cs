using Microsoft.EntityFrameworkCore;

namespace Tiantong.Iot.Entities
{
  public class IotDbContext : DBCore.DbContext
  {
    public DbSet<Plc> Plcs { get; set; }

    public DbSet<PlcLog> PlcLogs { get; set; }

    public DbSet<PlcError> PlcErrors { get; set; }

    public DbSet<PlcState> PlcStates { get; set; }

    public DbSet<PlcStateLog> PlcStateLogs { get; set; }

    public DbSet<PlcStateError> PlcStateErrors { get; set; }

    public DbSet<HttpWatcher> HttpWatchers { get; set; }

    public DbSet<HttpWatcherLog> HttpWatcherLogs { get; set; }

    public DbSet<HttpWatcherError> HttpWatcherErrors { get; set; }

  }
}
