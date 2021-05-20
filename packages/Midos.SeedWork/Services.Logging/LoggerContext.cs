using Microsoft.EntityFrameworkCore;
using Midos.SeedWork.Domain;

namespace Midos.SeedWork.Services.Logging
{
  public class LoggerContext: EFContext
  {
    public DbSet<LogData> LogData { get; set; }

    public LoggerContext(EFContextOptions<LoggerContext> options)
      : base(options) {}
  }
}
