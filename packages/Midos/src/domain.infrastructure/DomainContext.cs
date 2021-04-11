using DBCore;

namespace Midos.Domain
{
  public class DomainContext: DbContext
  {
    protected IEventPublisher Publisher;

    protected IDomainContextOptions Options;

    public DomainContext(IDomainContextOptions options, IEventPublisher publisher)
    {
      Options = options;
      Publisher = publisher;
    }

    public void Publish(string name, object data)
      => Publisher.Publish(name, data);

    protected override void OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder builder)
      => Options.OnConfiguring(builder);
  }
}
