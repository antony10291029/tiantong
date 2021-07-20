using Microsoft.EntityFrameworkCore;
using Midos.Center.Entities;
using Midos.Domain;

namespace Midos.Center
{
  public class ServiceContext: DomainContext
  {
    protected DbSet<App> Apps { get; set; }

    protected DbSet<Config> Configs { get; set; }

    public ServiceContext(IDomainContextOptions<DomainContext> options, IEventPublisher publisher)
      : base(options, publisher)
    {

    }
  }
}
