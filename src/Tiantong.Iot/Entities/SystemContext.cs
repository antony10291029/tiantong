using Microsoft.EntityFrameworkCore;

namespace Tiantong.Iot.Entities
{
  public class SystemContext: DBCore.DbContext
  {
    public DbSet<HttpPusher> HttpPushers { get; set; }

    public DbSet<KeyValue> KeyValues { get; set; }

    public DbSet<Plc> Plcs { get; set; }

    public DbSet<PlcState> PlcStates { get; set; }

    public DbSet<PlcStateHttpPusher> PlcStateHttpPushers { get; set; }

  }

}
