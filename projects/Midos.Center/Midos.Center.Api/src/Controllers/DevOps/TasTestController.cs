using Microsoft.AspNetCore.Mvc;
using Midos.Center.Utils;
using Midos.Center.Events;
using Midos.Domain;
using DotNetCore.CAP;

namespace Midos.Center.Controllers
{
  public class TasTestController: BaseController
  {
    private TaskService _tasks;

    public TasTestController(TaskService tasks)
    {
      _tasks = tasks;
    }

    [TaskStarted("$tas.test", Group = "tas.test")]
    public void HandleTaskCreated(TaskOrderChanged param)
    {
      _tasks.Finish(param.OrderId, new Record() {
        { "message", "Hello World" }
      });
    }
  }
}
