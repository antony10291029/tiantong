using Microsoft.EntityFrameworkCore;
using Namei.Common.Entities;

namespace Namei.Common.Api
{
  public class SapContext: DbContext
  {
    private Config _config;

    public DbSet<SapItemBatch> ItemBatches { get; set; }

    public SapContext(Config config)
    {
      _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      options.UseSqlServer(_config.SapUrl);
    }
  }
}
