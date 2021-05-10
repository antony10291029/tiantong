using Microsoft.EntityFrameworkCore;
using Midos.Domain;

namespace Midos.Services.Logging
{
  public class LoggerContext: DomainContext
  {
    public DbSet<LogData> LogData { get; set; }

    public LoggerContext(
      IDomainContextOptions<LoggerContext> options,
      IEventPublisher publisher
    ): base(options, publisher) {}

  }
}
