using Microsoft.AspNetCore.Mvc;
using Midos.Domain;
using Midos.Eventing;

namespace Midos.Server.Controllers
{
  public class DevController: BaseController
  {
    private readonly DomainContext _domain;

    public DevController(DomainContext domain)
    {
      _domain = domain;
    }

    public class TestMessage
    {
      public int Id { get; set; }

      public string Key { get; set; }

      public string Value { get; set; }
    }

    [HttpPost("/midos/cap/test")]
    public INotifyResult<IMessageObject> Test()
    {
      _domain.Publish("midos.test", new TestMessage {
        Id = 1000,
        Key = "test.key",
        Value = "test.value"
      });

      return NotifyResult.FromVoid().Success("消息发送成功");
    }

    [EventSubscribe("midos.test", "test")]
    public void Handle(TestMessage msg)
    {
      System.Console.WriteLine("================ Success ====================");
      System.Console.WriteLine(msg.Id);
      System.Console.WriteLine(msg.Key);
      System.Console.WriteLine(msg.Value);
    }
  }
}
