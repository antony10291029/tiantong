using DotNetCore.CAP;

namespace Midos.Domain
{
  public interface IEventPublisher
  {
    void Publish(string msg, object data);

    void Publish<T>(string msg, T data) where T: DomainEvent;
  }

  public class EventPublisher: IEventPublisher
  {
    private ICapPublisher _cap;

    public EventPublisher(ICapPublisher cap)
    {
      _cap = cap;
    }

    public void Publish(string msg, object data)
      => _cap.Publish(msg, data);

    public void Publish<T>(string msg, T data) where T: DomainEvent
      => _cap.Publish(msg, data);
  }
}
