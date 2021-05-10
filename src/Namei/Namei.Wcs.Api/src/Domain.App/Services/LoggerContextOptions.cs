using Microsoft.EntityFrameworkCore;
using Midos.Domain;
using Midos.Services.Logging;
using Namei.Wcs.Api;

namespace Namei.Wcs.Aggregates
{
  public class LoggerContextOptions: DomainContextOptions<LoggerContext>
  {
    private readonly IAppConfig _config;

    public LoggerContextOptions(IAppConfig config)
    {
      _config = config;
    }

    public override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseNpgsql(_config.MidosLoggerContext);
    }
  }
}
