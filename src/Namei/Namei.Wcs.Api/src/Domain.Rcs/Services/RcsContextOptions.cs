using Microsoft.EntityFrameworkCore;
using Midos.Domain;
using Namei.Wcs.Api;

namespace Namei.Wcs.Aggregates
{
  public class RcsContextOptions: DomainContextOptions<RcsContext>
  {
    private readonly IAppConfig _config;

    public RcsContextOptions(IAppConfig config)
    {
      _config = config;
    }

    public override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseNpgsql(_config.RcsContext);
    }
  }
}
