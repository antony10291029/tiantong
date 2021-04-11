using DotNetCore.CAP;

namespace Midos.Domain
{
  public interface IEventPublisher
  {
    void Publish(string msg, object data);
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
  }
}
