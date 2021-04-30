using Microsoft.AspNetCore.Mvc;
using Namei.Wcs.Api;

namespace Namei.Wcs.Aggregates
{
  public class AgcTaskCallbackController: BaseController
  {
    private readonly IAgcTaskService _agcTasks;

    private readonly Logger _logger;

    public AgcTaskCallbackController(
      IAgcTaskService agcTasks,
      Logger logger
    ) {
      _agcTasks = agcTasks;
      _logger = logger;
    }

    public record AgcCallbackParams
    {
      public string CallCode { get; set; }

      public int CooX{ get; set; }

      public int CooY{ get; set; }

      public string CurrentCallCode { get; set; }

      public string CurrentPositionCode { get; set; }

      public string Data { get; set; }

      public string MapCode { get; set; }

      public string MapDataCode { get; set; }

      public string Method { get; set; }

      public string ReqCode { get; set; }

      public string ReqTime { get; set; }

      public string RobotCode { get; set; }

      public string TaskCode { get; set; }

      public string WbCode { get; set; }
    }

    public struct RcsCallbackResult
    {
      public string Code { get; set; }

      public string Message { get; set; }

      public string ReqCode { get; set; }
    }

    [HttpPost("/rcs/agc-callback")]
    public IResult<RcsCallbackResult> RcsCallback([FromBody] AgcCallbackParams param)
    {
      _logger.Save(Log.From(
        Log.UseInfo(),
        Log.UseMessage("收到 RCS 任务反馈信息"),
        Log.UseClass("rcs.tasks"),
        Log.UseOperation("callback"),
        Log.UseIndex(param.TaskCode),
        Log.UseData(param)
      ));

      var result = new RcsCallbackResult {
        Code = "0",
        ReqCode = param.ReqCode
      };

      if (param.Method == "finish") {
        var task = _agcTasks.FindByTaskCode(param.TaskCode);

        if (task == null) {
          result.Message = "任务编号不存在";
        } else if (task.Status == AgcTaskStatus.Created) {
          _agcTasks.Finish(AgcTaskFinish.From(
            id: task.Id,
            agcCode: param.RobotCode
          ));

          result.Message = "任务状态已更新";
        }
      } else {
        result.Message = "操作无须处理";
      }

      return Result.From(result).StatusCode(200);
    }
  }
}
