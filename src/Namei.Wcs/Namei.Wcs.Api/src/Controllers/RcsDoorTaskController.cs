using DotNetCore.CAP;
using Renet.Web;

namespace Namei.Wcs.Api
{
  public class RcsDoorTaskController: BaseController
  {
    const string Group = "rcs.door.task";

    private ICapPublisher _cap;

    private DoorTaskManager _tasks;

    public RcsDoorTaskController(ICapPublisher cap, DoorTaskManager tasks)
    {
      _cap = cap;
      _tasks = tasks;
    }

    [CapSubscribe(DoorTaskRequestOpenEvent.Message, Group = Group)]
    public void HandleRequestOpen(DoorTaskRequestOpenEvent param)
    {
      _tasks.Tasks[param.DoorId].Request(param.TaskId);
    }

    [CapSubscribe(DoorTaskHandleEvent.Message, Group = Group)]
    public void HandleDoorTask(DoorOpenedEvent param)
    {
      _tasks.Tasks[param.DoorId].Handle();
    }

    [CapSubscribe(DoorOpenedEvent.Message, Group = Group)]
    public void HandleDoorOpened(DoorOpenedEvent param)
    {
      _tasks.Tasks[param.DoorId].Enter();
    }

    [CapSubscribe(DoorTaskRequestCloseEvent.Message, Group = Group)]
    public void HandleRequestClose(DoorTaskRequestCloseEvent param)
    {
      _tasks.Tasks[param.DoorId].Leave(param.TaskId);
    }
  }
}