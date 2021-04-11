using System;
using DBCore;

namespace Midos.Domain
{
  public class DomainContext: DbContext
  {
    protected IEventPublisher Publisher;

    public DomainContext(IEventPublisher publisher)
    {
      Publisher = publisher;
    }

    public void Publish(string name, object data)
      => Publisher.Publish(name, data);
  }
}
