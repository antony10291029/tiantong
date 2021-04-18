using System.Collections.Generic;

namespace Midos.Domain.Test
{
  public class TestEventPublisher: IEventPublisher
  {
    public static Dictionary<string, List<DomainEvent>> events
      = new Dictionary<string, List<DomainEvent>>();

    public void Publish(string name, object data)
    {
      throw new System.Exception("Not completed");
    }

    public void Publish<T>(string name, T data) where T: DomainEvent
    {
      if (!events.ContainsKey(name)) {
        events[name] = new List<DomainEvent>();
      }

      events[name].Add((DomainEvent )data);
    }

  }
}
