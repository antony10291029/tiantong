using Microsoft.EntityFrameworkCore;
using Midos.Domain;
using Namei.Wcs.Api;
using System.Collections.Generic;
using System.Linq;

namespace Namei.Wcs.Aggregates
{
  public interface IRcsAgcTaskService
  {
    void Create(RcsAgcTaskCreate param);

    void Start(RcsAgcTaskStart param);

    void Started(RcsAgcTaskStarted param);

    void Close(RcsAgcTaskClose param);

    void Finish(RcsAgcTaskFinish param);

    void Finished(RcsAgcTaskFinished param);

    IPagination<RcsAgcTask> Search(IQueryParams param);
  }

  public class RcsAgcTaskService: IRcsAgcTaskService
  {
    private Namei.Wcs.Api.DomainContext _domain;

    private RcsService _rcs;

    public RcsAgcTaskService(Namei.Wcs.Api.DomainContext domain, RcsService rcs)
    {
      _domain = domain;
      _rcs = rcs;
    }

    public void Create(RcsAgcTaskCreate param)
    {
      _domain.Add(RcsAgcTask.From(param));
      _domain.SaveChanges();
      _domain.Publish(RcsAgcTaskCreated.Message, RcsAgcTaskCreated.From(
        id: param.OrderId
      ));
    }

    public void Start(RcsAgcTaskStart param)
    {
      var task = _domain.Find<RcsAgcTask>(param.Id);

      if (task.Status != RcsAgcTaskStatus.Created) {
        return;
      }

      var result = _rcs.CreateTask(new RcsTaskCreateParams {
        taskTyp = task.TaskType,
        agvCode = task.AgcCode,
        positionCodePath = new List<PositionCodePath> {
          new PositionCodePath { positionCode = task.Position, type = "00" },
          new PositionCodePath { positionCode = task.Destination, type = "00" },
        }
      });

      _domain.Publish(RcsAgcTaskStarted.Message, RcsAgcTaskStarted.From(
        id: param.Id,
        taskCode: result.code
      ));
    }

    public void Started(RcsAgcTaskStarted param)
    {
      var task = _domain.Find<RcsAgcTask>(param.Id);

      task.Start(param.TaskCode);
      _domain.SaveChanges();
    }

    public void Close(RcsAgcTaskClose param)
    {
      var task = _domain.Find<RcsAgcTask>(param.Id);

      task.Close();
      _domain.SaveChanges();
    }

    public void Finish(RcsAgcTaskFinish param)
    {
      var task = _domain.Find<RcsAgcTask>(param.Id);

      _domain.Publish(RcsAgcTaskFinish.Message, param);

      if (task.OrderType != "") {
        _domain.Publish(
          name: RcsAgcTaskOrderFinished.Message(task.OrderType),
          data: RcsAgcTaskOrderFinished.From(
            orderId: task.OrderId,
            agcCode: param.AgcCode,
            podCode: task.PodCode
          )
        );
      }
    }

    public void Finished(RcsAgcTaskFinished param)
    {
      var task = _domain.Find<RcsAgcTask>(param.Id);

      task.Finish(param.AgcCode);
      _domain.SaveChanges();
      _domain.Publish(RcsAgcTaskFinished.Message, RcsAgcTaskFinished.From(
        id: param.Id,
        agcCode: param.AgcCode
      ));
    }

    public IPagination<RcsAgcTask> Search(IQueryParams param)
    {
      var query = _domain.Set<RcsAgcTask>().AsQueryable();

      if (param.Query != null && param.Query != "") {
        query = query.Where(task =>
          task.OrderType.Contains(param.Query) ||
          task.TaskCode.Contains(param.Query)
        );
      }

      return query
        .OrderByDescending(task => task.CreatedAt)
        .ThenByDescending(task => task.Id)
        .Paginate(param);
    }

  }
}
