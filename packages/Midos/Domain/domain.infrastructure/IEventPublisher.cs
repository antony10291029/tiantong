using System.Threading.Tasks;

namespace Midos.Domain
{
  public interface IEventPublisher
  {
    void Publish(string msg, object data);

    void Publish<T>(string msg, T data) where T: DomainEvent;
  }

  public class EventPublisher: IEventPublisher
  {
    private readonly Eventing.IEventPublisher _publisher;

    public EventPublisher(Eventing.IEventPublisher publisher)
    {
      _publisher = publisher;
    }

    public void Publish(string msg, object data)
      => _publisher.PublishAsync(msg, data).GetAwaiter().GetResult();

    public void Publish<T>(string msg, T data) where T: DomainEvent
      => _publisher.PublishAsync(msg, data).GetAwaiter().GetResult();
  }
}
