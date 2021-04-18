using Microsoft.EntityFrameworkCore;

namespace Midos.Domain
{
  public class DomainContext: EFContext
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

    public void Publish<T>(string name, T data) where T: DomainEvent
      => Publisher.Publish(name, data);

    protected override void OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder builder)
      => Options.OnConfiguring(builder);
  }
}
