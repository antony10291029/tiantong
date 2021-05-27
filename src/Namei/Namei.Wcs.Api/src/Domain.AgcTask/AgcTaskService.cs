using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Midos.Domain;
using Midos.Services.Http;
using Namei.Wcs.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Namei.Wcs.Aggregates
{
  public class AgcTaskCreateResult: IMessageObject
  {
    public long Id { get; set; }

    public int Code { get; set; }

    public string Message { get; set; }
  }

  public interface IAgcTaskService
  {
    AgcTaskCreateResult Create(AgcTaskCreate param);

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

    private readonly IRcsMapService _rcsMap;

    public AgcTaskService(WcsContext domain, IRcsService rcs, IRcsMapService rcsMap)
    {
      _context = domain;
      _rcs = rcs;
      _rcsMap = rcsMap;
    }

    private static string GetAreaCode(string code)
      => code.EndsWith("${04}") ? code.Split("$").First() : null;

    private RcsTaskCreateResult Start(AgcTask task, AgcTaskType type)
    {
      if (type is null) {
        throw KnownException.Error("任务类型不存在");
      } else if (type.IsEnabled == false) {
        throw KnownException.Error("任务类型已禁用");
      }

      var codes = _rcsMap.ToDataName(new string[] { task.Position, task.Destination });
      var areaCode = GetAreaCode(codes[1]);

      if (areaCode != null) {
        codes[1] = _rcsMap.GetFreeLocationCode(areaCode);

        if (codes[1] == null) {
          throw KnownException.Error("区域内无可分配点位");
        }
      }

      var result = _rcs.CreateTask(new RcsTaskCreateParams {
        TaskTyp = type.Method,
        AgvCode = task.AgcCode,
        PodCode = task.PodCode,
        Priority = task.Priority,
        PositionCodePath = new List<PositionCodePath> {
          new PositionCodePath { PositionCode = codes[0], Type = "00" },
          new PositionCodePath { PositionCode = codes[1], Type = "00" },
        }
      }).GetAwaiter().GetResult();

      return result;
    }

    public AgcTaskCreateResult Create(AgcTaskCreate param)
    {
      AgcTaskType type;

      if (param.TypeId != default(int)) {
        type = _context.Set<AgcTaskType>().First(type => type.Id == param.TypeId);
      } else {
        type = _context.Set<AgcTaskType>().First(type => type.Key == param.Type);
        param.TypeId = type.Id;
      }

      if (type is null) {
        throw KnownException.Error("任务类型不存在");
      } else if (type.IsEnabled == false) {
        throw KnownException.Error("任务类型已禁用");
      }

      var task = AgcTask.From(param);
      var result = Start(task, type);

      if (result.Code == "0") {
        task.Start(result.Data);
        _context.Add(task);
        _context.SaveChanges();
      }

      return new AgcTaskCreateResult {
        Id = task.Id,
        Code = int.Parse(result.Code),
        Message = result.Message
      };
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
