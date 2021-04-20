using Microsoft.AspNetCore.Mvc;
using Namei.Wcs.Api;

namespace Namei.Wcs.Aggregates
{
  public class RcsAgcTaskCallbackController: BaseController
  {
    private IRcsAgcTaskService _rcs;

    private Logger _logger;

    public RcsAgcTaskCallbackController(
      IRcsAgcTaskService rcs,
      Logger logger
    ) {
      _rcs = rcs;
      _logger = logger;
    }

    public class AgcCallbackParams
    {
      public string callCode { get; set; }

      public int cooX{ get; set; }

      public int cooY{ get; set; }

      public string currentCallCode { get; set; }

      public string currentPositionCode { get; set; }

      public string data { get; set; }

      public string mapCode { get; set; }

      public string mapDataCode { get; set; }

      public string method { get; set; }

      public string reqCode { get; set; }

      public string reqTime { get; set; }

      public string robotCode { get; set; }

      public string taskCode { get; set; }

      public string wbCode { get; set; }
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
        Log.UseIndex(param.taskCode),
        Log.UseData(param)
      ));

      var result = new RcsCallbackResult {
        Code = "0",
        ReqCode = param.reqCode
      };

      if (param.method == "finish") {
        var task = _rcs.FindByTaskCode(param.taskCode);

        if (task == null) {
          result.Message = "任务编号不存在";
        } else if (task.Status == RcsAgcTaskStatus.Started) {
          _rcs.Finish(RcsAgcTaskFinish.From(
            id: task.Id,
            agcCode: param.robotCode
          ));

          result.Message = "任务状态已更新";
        }
      } else {
        result.Message = "操作无须处理";
      }

      return Result.From(result);
    }
  }
}
