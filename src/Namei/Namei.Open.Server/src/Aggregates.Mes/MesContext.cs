using Microsoft.EntityFrameworkCore;

namespace Namei.Open.Server
{
  public class MesContext: DbContext
  {
    private readonly AppConfig _config;

    protected DbSet<MesRetrospectCode> RetrospectCodes { get; set; }

    protected DbSet<MesWoOrder> MesWoOrders { get; set; }

    public MesContext(AppConfig config)
    {
      _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      options.UseSqlServer(_config.MesDatabase);
    }
  }
}
