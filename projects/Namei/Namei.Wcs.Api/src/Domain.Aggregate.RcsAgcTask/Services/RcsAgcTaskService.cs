using Midos.Domain;
using Namei.Wcs.Api;
using System.Collections.Generic;
using System.Linq;

namespace Namei.Wcs.Aggregates
{
  public interface IRcsAgcTaskService
  {
    long Create(RcsAgcTaskCreate param);

    void Start(RcsAgcTaskStart param);

    void Started(RcsAgcTaskStarted param);

    void Close(RcsAgcTaskClose param);

    void Finish(RcsAgcTaskFinish param);

    void Finished(RcsAgcTaskFinished param);

    RcsAgcTask FindByTaskCode(string taskCode);

    IPagination<RcsAgcTask> Search(IQueryParams param);
  }

  public class RcsAgcTaskService: IRcsAgcTaskService
  {
    private WcsContext _domain;

    private IRcsService _rcs;

    public RcsAgcTaskService(WcsContext domain, IRcsService rcs)
    {
      _domain = domain;
      _rcs = rcs;
    }

    public long Create(RcsAgcTaskCreate param)
    {
      var task = RcsAgcTask.From(param);

      _domain.Add(task);
      _domain.SaveChanges();
      _domain.Publish(RcsAgcTaskCreated.Message, RcsAgcTaskCreated.From(
        id: task.Id
      ));

      return task.Id;
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
        taskCode: result.data
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

      task.Finish(param.AgcCode);

      _domain.SaveChanges();
      _domain.Publish(RcsAgcTaskFinished.Message, RcsAgcTaskFinished.From(
        id: param.Id
      ));
    }

    public void Finished(RcsAgcTaskFinished param)
    {
      var task = _domain.Find<RcsAgcTask>(param.Id);

      if (task.OrderType != "") {
        _domain.Publish(
          name: RcsAgcTaskOrderFinished.Message(task.OrderType),
          data: RcsAgcTaskOrderFinished.From(
            id: task.Id,
            orderId: task.OrderId,
            agcCode: task.AgcCode,
            podCode: task.PodCode
          )
        );
      }
    }

    public RcsAgcTask FindByTaskCode(string taskCode)
    {
      return _domain.Set<RcsAgcTask>().FirstOrDefault(task => task.TaskCode == taskCode);
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
