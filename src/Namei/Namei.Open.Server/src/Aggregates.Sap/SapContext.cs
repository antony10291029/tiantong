using Microsoft.EntityFrameworkCore;

namespace Namei.Open.Server
{
  public class SapContext: DbContext
  {
    private readonly AppConfig _config;

    protected DbSet<SapItem> Items { get; set; }

    public SapContext(AppConfig config)
    {
      _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      options.UseSqlServer(_config.SapDatabase);
    }
  }
}
