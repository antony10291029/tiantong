using DotNetCore.CAP;
using Midos.Center.Events;
using Midos.Domain;

namespace Midos.Center.Utils
{
  public class TaskService
  {
    private ICapPublisher _cap;

    public TaskService(ICapPublisher cap)
    {
      _cap = cap;
    }

    public void Create(string key, Record data)
      => _cap.Publish(
        TaskOrderCreate.Message,
        TaskOrderCreate.From(key, data)
      );

    public void Create(long orderId, string subkey, Record data)
      => _cap.Publish(
        SubtaskOrderCreate.Message,
        SubtaskOrderCreate.From(orderId, subkey, data)
      );

    public void Change(string method, long orderId, Record data)
      => _cap.Publish(
        method,
        TaskOrderChange.From(orderId, data)
      );

    public void Start(long orderId, Record data)
      => Change(
        method: TaskOrderChange.Start,
        orderId: orderId,
        data: data
      );

    public void Finish(long orderId, Record data)
      => Change(
        method: TaskOrderChange.Finish,
        orderId: orderId,
        data: data
      );

    public void Cancel(long orderId, Record data)
      => Change(
        method: TaskOrderChange.Cancel,
        orderId: orderId,
        data: data
      );
  }
}
