using Microsoft.AspNetCore.Mvc;
using DotNetCore.CAP;

namespace Midos.Server.Controllers
{
  public class DevController: BaseController
  {
    private ICapPublisher _cap;

    public DevController(ICapPublisher cap)
    {
      _cap = cap;
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
      _cap.Publish("midos.test", new TestMessage {
        Id = 1000,
        Key = "test.key",
        Value = "test.value"
      });

      return NotifyResult.FromVoid().Success("消息发送成功");
    }

    [CapSubscribe("midos.test", Group = "test")]
    public void Handle(TestMessage msg)
    {
      System.Console.WriteLine("================ Success ====================");
      System.Console.WriteLine(msg.Id);
      System.Console.WriteLine(msg.Key);
      System.Console.WriteLine(msg.Value);
    }
  }
}
