using Microsoft.EntityFrameworkCore;
using Namei.Wcs.Database;

namespace Namei.Wcs.Api
{
  public class DomainContext: PostgresContext
  {
    private Config _config;

    public DbSet<Log> Logs { get; set; }

    public DomainContext(Config config)
    {
      _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      options.UseNpgsql(_config.PG_CONNECTION);
    }
  }
}
