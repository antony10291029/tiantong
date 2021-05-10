using Microsoft.EntityFrameworkCore;
using Midos.Domain;
using Midos.Services.Logging;

namespace Namei.ApiGateway.Server
{
  public class LoggerContextOptions: DomainContextOptions<LoggerContext>
  {
    private readonly AppConfig _config;

    public LoggerContextOptions(AppConfig config)
    {
      _config = config;
    }

    public override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseNpgsql(_config.LoggerContext);
    }
  }
}
