using Tiantong.Iot.Entities;
using Microsoft.EntityFrameworkCore;

namespace Tiantong.Iot.Database
{
  public class DbContext : DBCore.DbContext
  {
    public DbSet<Tiantong.Iot.Entities.Plc> Plcs { get; set; }

    public DbSet<PlcConnectionLog> PlcConnectionLogs { get; set; }

    public DbSet<PlcState> PlcStates { get; set; }

    public DbSet<PlcStateLog> PlcStateLogs { get; set; }
  }
}
