using Microsoft.EntityFrameworkCore;
using Midos.Domain;
using Namei.Common.Api;

namespace Namei.Aggregates
{
  public class AppContextOptions: DomainContextOptions<AppContext>
  {
    private readonly Config _config;

    public AppContextOptions(Config config)
    {
      _config = config;
    }

    public override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseNpgsql(_config.WcsDb);
    }
  }

  public class AppContext: DomainContext
  {
    protected DbSet<OrderItemReviewRecord> OrderItemReviewRecords { get; set; }

    public AppContext(
      IDomainContextOptions<AppContext> options,
      IEventPublisher publisher
    ): base(options, publisher) {}
  }
}
