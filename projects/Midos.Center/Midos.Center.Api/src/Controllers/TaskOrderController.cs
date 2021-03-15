using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Midos.Center.Entities;
using Midos.Center.Events;
using System.Linq;

namespace Midos.Center.Controllers
{
  public class TaskOrderController: BaseController
  {
    public const string Group = "task.order";

    private ICapPublisher _cap;

    private DomainContext _domain;

    public TaskOrderController(
      DomainContext domain,
      ICapPublisher cap
    ) {
      _cap = cap;
      _domain = domain;
    }

    [CapSubscribe(TaskOrderCreate.Message, Group = Group)]
    public void HandleTaskOrderCreate(TaskOrderCreate param)
    {
      var type = _domain.TaskTypes.First(type => type.Key == param.Key);
      var order = TaskOrder.From(type, param.Data);

      _domain.Add(order);
      _domain.SaveChanges();

      var message = TaskOrderCreated.MessageFrom(type);
      var data = TaskOrderCreated.From(order);

      _cap.Publish(message, TaskOrderCreated.From(order));
    }
  }
}
