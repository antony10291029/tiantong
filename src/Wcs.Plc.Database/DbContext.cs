using Wcs.Plc.Entities;
using Microsoft.EntityFrameworkCore;

namespace Wcs.Plc.Database
{
  public class DbContext : DBCore.DbContext
  {
    public DbSet<PlcConnection> PlcConnections { get; set; }

    public DbSet<PlcConnectionLog> PlcConnectionLogs { get; set; }

    public DbSet<PlcState> PlcStates { get; set; }

    public DbSet<PlcStateLog> PlcStateLogs { get; set; }
  }
}
