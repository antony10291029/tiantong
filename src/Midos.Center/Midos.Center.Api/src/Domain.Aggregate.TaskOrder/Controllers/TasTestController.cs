using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Midos.Center.Utils;
using Midos.Center.Events;
using Midos.Domain;

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
    public void HandleTaskStarged(TaskOrderChanged param)
    {
      _cap.Publish(SubtaskOrderCreate.Message, SubtaskOrderCreate.From(
        orderId: param.OrderId,
        subkey: "stage1",
        data: new Record() {
          { "message", "stage_1_started" },
          { "AgvCode", param.Data["AgvCode"] }
        }
      ));
    }

    [TaskStarted("$tas.test.subtask", Group = "tas.test")]
    public void HandleStage1Started(TaskOrderChanged param)
    {
      _cap.Publish(TaskOrderChange.Finish, TaskOrderChange.From(
        orderId: param.OrderId,
        data: new Record() {}
      ));
    }

    [SubtaskFinished("$tas.test", "stage1", Group = "tas.test")]
    public void HandleStage1Finished(TaskOrderChange param)
    {
      _cap.Publish(TaskOrderChange.Finish, TaskOrderChange.From(
        orderId: param.OrderId,
        data: param.Data
      ));
    }
  }
}
