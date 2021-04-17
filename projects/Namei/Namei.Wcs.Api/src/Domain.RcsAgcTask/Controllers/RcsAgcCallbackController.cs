using Microsoft.AspNetCore.Mvc;
using Midos.Domain;
using Namei.Wcs.Api;

namespace Namei.Wcs.Aggregates
{
  public class RcsAgcCallbackController: BaseController
  {
    private IRcsAgcTaskService _rcsTaskService;

    private Logger _logger;

    public RcsAgcCallbackController(
      IRcsAgcTaskService rcsTaskService,
      Logger logger
    ) {
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

    [HttpPost("/rcs/agc-callback")]
    public object RcsCallback([FromBody] AgcCallbackParams param)
    {
      _logger.Save(Log.From(
        Log.UseInfo(),
        Log.UseMessage("收到 RCS 任务反馈信息"),
        Log.UseClass("rcs.tasks"),
        Log.UseOperation("callback"),
        Log.UseIndex(param.taskCode),
        Log.UseData(param)
      ));

      if (param.method == "finish") {
        var task = _rcsTaskService.FindByTaskCode(param.taskCode);

        _rcsTaskService.Finish(RcsAgcTaskFinish.From(
          id: task.Id,
          agcCode: param.robotCode
        ));
      }

      return new {
        code = "0",
        message = "信息已接受"
      };
    }
  }
}
