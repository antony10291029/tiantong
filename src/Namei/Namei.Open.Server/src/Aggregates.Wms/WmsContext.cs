using Microsoft.EntityFrameworkCore;

namespace Namei.Open.Server
{
  public class WmsContext: DbContext
  {
    private readonly AppConfig _config;

    protected DbSet<WmsMoveDoc> WmsMoveDocs { get; set; }

    protected DbSet<WmsPickTicket> WmsPickTickets { get; set; }

    protected DbSet<WmsBoxCodeBind> WmsBoxCodeBinds { get; set; }

    public WmsContext(AppConfig config)
    {
      _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      options.UseMySQL(_config.WmsDatabase);
    }
  }
}
