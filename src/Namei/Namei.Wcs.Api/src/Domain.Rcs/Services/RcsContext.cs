using Microsoft.EntityFrameworkCore;
using Midos.Domain;

namespace Namei.Wcs.Aggregates
{
  public class RcsContext: DomainContext
  {
    public DbSet<TcsMainTask> MainTasks { get; set; }

    public DbSet<TcsMapData> MapData { get; set; }

    public RcsContext(
      IDomainContextOptions<RcsContext> options,
      IEventPublisher publisher
    ) : base(options, publisher) {}
  }
}
