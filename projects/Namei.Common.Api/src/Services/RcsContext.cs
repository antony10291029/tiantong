using Microsoft.EntityFrameworkCore;

namespace Namei.Common.Api
{
  public class RcsContext: DbContext
  {
    private Config _config;

    public DbSet<MapData> MapData { get; set; }

    public RcsContext(Config config)
    {
      _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      options.UseNpgsql(_config.RcsDbConfig);
    }
  }
}
