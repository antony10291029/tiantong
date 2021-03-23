using Microsoft.EntityFrameworkCore;
using Namei.Common.Entities;

namespace Namei.Common.Api
{
  public class WmsContext: DbContext
  {
    private Config _config;

    public DbSet<WmsAsn> Asns { get; private set; }

    public DbSet<WmsAsnDetail> AsnDetails { get; private set; }

    public DbSet<WmsItem> Items { get; private set; }

    public WmsContext(Config config)
    {
      _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      options.UseMySQL(_config.WmsUrl);
    }
  }
}
