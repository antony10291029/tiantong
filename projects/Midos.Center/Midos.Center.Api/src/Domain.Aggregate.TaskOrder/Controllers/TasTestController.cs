using Microsoft.AspNetCore.Mvc;
using Midos.Center.Utils;
using Midos.Center.Events;
using Midos.Domain;
using DotNetCore.CAP;

namespace Midos.Center.Controllers
{
  public class TasTestController: BaseController
  {
    private ICapPublisher _cap;

    public TasTestController(ICapPublisher cap)
    {
      _cap = cap;
    }

    [TaskStarted("$tas.test", Group = "tas.test")]
    public void HandleTaskCreated(TaskOrderChanged param)
    {
      _cap.Publish(TaskOrderChange.Finish, TaskOrderChange.From(
        orderId: param.OrderId,
        data: new Record() {
          { "message", "Hello World" }
        }
      ));
    }
  }
}
