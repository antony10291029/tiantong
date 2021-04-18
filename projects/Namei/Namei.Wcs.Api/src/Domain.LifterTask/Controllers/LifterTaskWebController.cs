using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Midos.Domain;
using System.Linq;

namespace Namei.Wcs.Api
{
  public class LifterTaskWebController: BaseController
  {
    private DomainContext _domain;

    private ILifterLogger _logger;

    private IWmsService _wms;

    public LifterTaskWebController(
      DomainContext domain,
      IWmsService wms,
      ILifterLogger logger
    ) {
      _domain = domain;
      _logger = logger;
      _wms = wms;
    }

    public class CloseParams
    {
      public long Id { get; set; }
    }

    [HttpPost("/lifter-tasks/close")]
    public INotifyResult<IMessageObject> UpdateStatus([FromBody] CloseParams param)
    {
      var task = _domain.Set<LifterTask>().Find(param.Id);
      var result = NotifyResult.FromVoid();

      if (task == null) {
        result.Danger("任务 Id 不存在");
      } else {
        task.Close();
        _domain.SaveChanges();
        result.Success("任务已关闭");
      }

      return result;
    }

    [HttpPost("/lifter-tasks/search")]
    public IPagination<LifterTask> SearchTasks([FromBody] QueryParams param)
    {
      var query =  _domain.Set<LifterTask>().AsQueryable();

      if (param.Query != null && param.Query != "") {
        query = query.Where(task =>
          task.Barcode.Contains(param.Query) ||
          task.TaskCode.Contains(param.Query)
        );
      }

      return query
        .OrderByDescending(task => task.Status == LifterTaskStatus.Exported)
        .ThenByDescending(task => task.Status == LifterTaskStatus.Imported)
        .ThenByDescending(task => task.ImportedAt)
        .Paginate(param);
    }

    public class LifterNotify
    {
      public string Method { get; set; }

      public string LiftCode { get; set; }

      public string Floor { get; set; }

      public long TaskCode { get; set; }

      public string BarCode { get; set; }

      public string Destination { get; set; }

      public string Operator { get; set; } = "WMS";
    }

    [Route("/finish")]
    [HttpPost("/lifter-tasks/create")]
    public object LiftersNotify([FromBody] LifterNotify param)
    {
      var message = "";

      if (param.Method == "deliver") {
        message = "收到创建提升机任务指令";

        _domain.Publish(LifterTaskImported.Message, LifterTaskImported.From(
          lifterId: param.LiftCode,
          floor: param.Floor,
          taskCode: param.TaskCode.ToString(),
          barcode: param.BarCode,
          destination: param.Destination
        ));
      } else if (param.Method == "pick") {
        message = "收到取货完成指令";

        _domain.Publish(LifterTaskTaken.Message, LifterTaskTaken.From(
          lifterId: param.LiftCode,
          floor: param.Floor
        ));
      } else {
        message = $"指令未识别：{param.Method}";

        _domain.Publish(LifterOperationError.Message, LifterOperationError.From(
          lifterId: param.LiftCode,
          floor: param.Floor,
          operation: "wms.finish",
          message: message
        ));
      }

      _logger.FromLifter(
        operation: "command.finish",
        lifterId: param.LiftCode,
        floor: param.Floor,
        message: ($"收到 {param.Operator} 放、取货指令"),
        data: System.Text.Json.JsonSerializer.Serialize(param),
        useLevel: Log.UseInfo()
      );

      var result = new {
        message = message,
        data = param
      };

      return Result.FromObject(result).StatusCode(201);
    }
  }
}
