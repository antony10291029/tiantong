using Microsoft.EntityFrameworkCore;
using Midos.SeedWork.Domain;
using Midos.SeedWork.Services.Logging;

namespace Namei.ApiGateway.TestServer
{
  public class LoggerContextOptions: EFContextOptions<LoggerContext>
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
