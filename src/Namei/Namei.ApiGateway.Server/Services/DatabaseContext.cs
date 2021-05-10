using Microsoft.EntityFrameworkCore;
using Midos.Domain;

namespace Namei.ApiGateway.Server
{
  public class DatabaseContext: DomainContext
  {
    protected DbSet<Endpoint> Endpoints { get; set; }

    protected DbSet<Route> Routes { get; set; }

    public DatabaseContext(
      IDomainContextOptions<DatabaseContext> options,
      IEventPublisher publisher
    ): base(options, publisher) {}
  }
}
