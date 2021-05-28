using Microsoft.EntityFrameworkCore;

namespace Namei.Wcs.Api
{
  public class DomainOptions: Midos.Domain.DomainContextOptions<DomainContext>
  {
    private readonly IAppConfig _config;

    public DomainOptions(IAppConfig config)
    {
      _config = config;
    }

    public override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseNpgsql(_config.Postgres);
    }
  }
}
