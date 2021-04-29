using Midos.Domain;

namespace Namei.Wcs.Aggregates
{
  public class AgcTaskType: IEntity, IAggregateRoot
  {
    public long Id { get; private set; }

    public string Key { get; private set; }

    public string Name { get; private set; }

    public string Method { get; private set; }

    public string Webhook { get; private set; }

    private AgcTaskType() {}

    public static AgcTaskType From(
      string key,
      string name,
      string method,
      string webhook
    ) => new() {
        Key = key,
        Name = name,
        Method = method,
        Webhook = webhook
      };

    public void Update(
      string key,
      string name,
      string method,
      string webhook
    ) {
      Key = key;
      Name = name;
      Method = method;
      Webhook = webhook;
    }
  }
}
