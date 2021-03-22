using DotNetCore.CAP;
using Midos.Center.Events;
using Midos.Center.Entities;

namespace Midos.Center.Utils
{
  public class TaskService
  {
    private ICapPublisher _cap;

    public TaskService(ICapPublisher cap)
    {
      _cap = cap;
    }

    public void Create(string key, TaskData data)
      => _cap.Publish(
        TaskOrderCreate.Message,
        TaskOrderCreate.From(key, data)
      );

    public void Create(long orderId, string subkey, TaskData data)
      => _cap.Publish(
        SubtaskOrderCreate.Message,
        SubtaskOrderCreate.From(orderId, subkey, data)
      );

    public void Change(string method, long orderId, TaskData data)
      => _cap.Publish(
        TaskOrderChange.Message(method),
        TaskOrderChange.From(orderId, data)
      );

    public void Start(long orderId, TaskData data)
      => Change(
        method: TaskOrderChange.Start,
        orderId: orderId,
        data: data
      );

    public void Finish(long orderId, TaskData data)
      => Change(
        method: TaskOrderChange.Finish,
        orderId: orderId,
        data: data
      );

    public void Cancel(long orderId, TaskData data)
      => Change(
        method: TaskOrderChange.Cancel,
        orderId: orderId,
        data: data
      );
  }
}
