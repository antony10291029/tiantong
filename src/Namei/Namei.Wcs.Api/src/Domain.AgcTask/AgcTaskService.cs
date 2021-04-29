using Microsoft.EntityFrameworkCore;
using Midos.Domain;
using Midos.Services.Http;
using Namei.Wcs.Api;
using System.Collections.Generic;
using System.Linq;

namespace Namei.Wcs.Aggregates
{
  public interface IAgcTaskService
  {
    long Create(AgcTaskCreate param);

    void Start(AgcTaskStart param);

    void Started(AgcTaskStarted param);

    void Close(AgcTaskClose param);

    void Finish(AgcTaskFinish param);

    void Finished(AgcTaskFinished param);

    AgcTask FindByTaskCode(string taskCode);

    IPagination<AgcTask> Search(IQueryParams param);
  }

  public class AgcTaskService: IAgcTaskService
  {
    private readonly WcsContext _context;

    private readonly IRcsService _rcs;

    public AgcTaskService(WcsContext domain, IRcsService rcs)
    {
      _context = domain;
      _rcs = rcs;
    }

    public long Create(AgcTaskCreate param)
    {
      if (param.TypeId == default(int)) {
        param.TypeId = _context.Set<AgcTaskType>()
          .First(type => type.Key == param.Type)
          .Id;
      }

      var task = AgcTask.From(param);

      _context.Add(task);
      _context.SaveChanges();
      _context.Publish(
        AgcTaskCreated.@event,
        AgcTaskCreated.From(task.Id)
      );

      return task.Id;
    }

    public void Start(AgcTaskStart param)
    {
      var task = _context.Set<AgcTask>()
        .Include(task => task.Type)
        .First(task => task.Id == param.Id);

      if (task.Status != AgcTaskStatus.Created) {
        return;
      }

      var result = _rcs.CreateTask(new RcsTaskCreateParams {
        taskTyp = task.Type.Method,
        agvCode = task.AgcCode,
        podCode = task.PodCode,
        priority = task.Priority,
        positionCodePath = new List<PositionCodePath> {
          new PositionCodePath { positionCode = task.Position, type = "00" },
          new PositionCodePath { positionCode = task.Destination, type = "00" },
        }
      });

      _context.Publish(
        AgcTaskStarted.@event,
        AgcTaskStarted.From(
          id: param.Id,
          taskCode: result.data
        )
      );
    }

    public void Started(AgcTaskStarted param)
    {
      var task = _context.Find<AgcTask>(param.Id);

      task.Start(param.TaskCode);
      _context.SaveChanges();
    }

    public void Close(AgcTaskClose param)
    {
      var task = _context.Find<AgcTask>(param.Id);

      task.Close();
      _context.SaveChanges();
    }

    public void Finish(AgcTaskFinish param)
    {
      var task = _context.Find<AgcTask>(param.Id);

      task.Finish(param.AgcCode);

      _context.SaveChanges();
      _context.Publish(
        AgcTaskFinished.@event,
        AgcTaskFinished.From(
          id: param.Id
        )
      );
    }

    public record AgcTaskCallback
    {
      public string Type { get; set; }

      public string TaskId { get; set; }

      public string AgvCode { get; set; }
    }

    public void Finished(AgcTaskFinished param)
    {
      var task = _context.Set<AgcTask>()
        .Include(task => task.Type)
        .FirstOrDefault(task => task.Id == param.Id);

      if (task.Type.Webhook != "") {
        _context.Publish(
          name: HttpPost.@event,
          data: HttpPost.From(
            url: task.Type.Webhook,
            data: new AgcTaskCallback {
              AgvCode = task.AgcCode,
              Type = task.Type.Key,
              TaskId = task.TaskId
            }
          )
        );
      }
    }

    public AgcTask FindByTaskCode(string taskCode)
    {
      return _context.Set<AgcTask>().FirstOrDefault(task => task.RcsTaskCode == taskCode);
    }

    public IPagination<AgcTask> Search(IQueryParams param)
    {
      var query = _context.Set<AgcTask>()
        .Include(task => task.Type)
        .AsQueryable();

      if (param.Query != null && param.Query != "") {
        query = query.Where(task =>
          task.Type.Key.Contains(param.Query) ||
          task.RcsTaskCode.Contains(param.Query)
        );
      }

      return query
        .OrderByDescending(task => task.CreatedAt)
        .ThenByDescending(task => task.Id)
        .Paginate(param);
    }
  }
}
