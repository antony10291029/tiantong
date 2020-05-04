using Tiantong.Iot.Entities;
using Microsoft.EntityFrameworkCore;

namespace Tiantong.Iot.Entities
{
  public class DbContext : DBCore.DbContext
  {
    public DbSet<Plc> Plcs { get; set; }

    public DbSet<PlcLogs> PlcLogs { get; set; }

    public DbSet<PlcState> PlcStates { get; set; }

    public DbSet<PlcStateLog> PlcStateLogs { get; set; }

    public DbSet<HttpWatcher> HttpWatchers { get; set; }

    public DbSet<HttpWatcherLog> HttpWatcherLogs { get; set; }

  }
}
