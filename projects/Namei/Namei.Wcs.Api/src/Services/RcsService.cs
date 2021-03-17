using System.Reflection.Metadata;
using System.Net.Cache;
using System.Collections.Generic;
using DotNetCore.CAP;
using System;
using System.Net.Mime;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Namei.Wcs.Api
{
  public class RcsTaskCreateParams
  {
    public string reqCode { get; set; }

    public string taskTyp { get; set; }

    public string podCode { get; set; }

    public string priority { get; set; }

    public string taskCode { get; set; }

    public string agvCode { get; set; }

    public string data { get; set; }

    public List<PositionCodePath> positionCodePath { get; set; }
  }
  public class RcsTaskContinueParams
  {
    public string reqCode { get; set; }

    public string podCode { get; set; }

    public string taskCode { get; set; }

    public string agvCode { get; set; }

    public PositionCodePath nextPositionCode { get; set; }
  }

  public class PositionCodePath
  {
    public string positionCode { get; set; }

    public string type { get; set; }
  }

  public class RcsTaskCreateResult
  {
    public string code { get; set; }

    public string message { get; set; }

    public string reqCode { get; set; }

    public string data { get; set; }
  }

  public class RcsTaskCancelParams
  {
    public string reqCode { get; set; }

    public string agvCode { get; set; }

    public string taskCode { get; set; }
  }

  public class RcsTaskCancelResult
  {
    public string code { get; set; }

    public string message { get; set; }

    public string reqCode { get; set; }
  }

  public class RcsService
  {
    private HttpClient _client;

    private ICapPublisher _cap;

    private Logger _logger;

    public RcsService(
      IHttpClientFactory factory,
      ICapPublisher cap,
      Config config,
      Logger logger
    ) {
      _cap = cap;
      _logger = logger;
      _client = factory.CreateClient();
      _client.Timeout = new TimeSpan(0, 0, 10);
      _client.BaseAddress = new System.Uri(config.RcsUrl);
      _client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json)
      );
    }

    public RcsTaskCreateResult CreateTask(RcsTaskCreateParams param)
    {
      if (param.reqCode == null || param.reqCode == "") {
        param.reqCode = System.Guid.NewGuid().ToString();
      }

      var json = JsonSerializer.Serialize(param);
      var content = new StringContent(json, Encoding.UTF8);

      _logger.Save(Log.From(
        Log.UseClass("rcs.tasks"),
        Log.UseOperation("create"),
        Log.UseIndex("0"),
        Log.UseMessage("收到 RCS 任务"),
        Log.UseData(json),
        Log.UseInfo()
      ));

      try {
        var response = _client.PostAsync("/rcs/services/rest/hikRpcService/genAgvSchedulingTask", content)
          .GetAwaiter().GetResult();
        var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        _logger.Save(Log.From(
          Log.UseClass("rcs.tasks"),
          Log.UseOperation("create"),
          Log.UseIndex("0"),
          Log.UseMessage("RCS 任务创建成功"),
          Log.UseData(result),
          Log.UseSuccess()
        ));

        return JsonSerializer.Deserialize<RcsTaskCreateResult>(result);
      } catch (Exception e) {
        _logger.Save(Log.From(
          Log.UseClass("rcs.tasks"),
          Log.UseOperation("create"),
          Log.UseIndex("0"),
          Log.UseMessage("RCS 任务创建失败"),
          Log.UseData(e.Message),
          Log.UseDanger()
        ));

        return new RcsTaskCreateResult() {
          code = "0",
          message = e.Message,
          reqCode = param.reqCode,
          data = "null"
        };
      }
    }

    public RcsTaskCreateResult ContinueTask(RcsTaskContinueParams param)
    {
      if (param.reqCode == null || param.reqCode == "") {
        param.reqCode = System.Guid.NewGuid().ToString();
      }

      var json = JsonSerializer.Serialize(param);
      var content = new StringContent(json, Encoding.UTF8);

      _logger.Save(Log.From(
        Log.UseClass("rcs.tasks"),
        Log.UseOperation("continue"),
        Log.UseIndex("0"),
        Log.UseMessage("收到 RCS 触发任务"),
        Log.UseData(json),
        Log.UseInfo()
      ));

      try {
        var response = _client.PostAsync("/rcs/services/rest/hikRpcService/continueTask", content)
          .GetAwaiter().GetResult();
        var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        _logger.Save(Log.From(
          Log.UseClass("rcs.tasks"),
          Log.UseOperation("continue"),
          Log.UseIndex("0"),
          Log.UseMessage("RCS 触发创建成功"),
          Log.UseData(result),
          Log.UseSuccess()
        ));

        return JsonSerializer.Deserialize<RcsTaskCreateResult>(result);
      } catch (Exception e) {
        _logger.Save(Log.From(
          Log.UseClass("rcs.tasks"),
          Log.UseOperation("continue"),
          Log.UseIndex("0"),
          Log.UseMessage("RCS 任务触发失败"),
          Log.UseData(e.Message),
          Log.UseDanger()
        ));

        return new RcsTaskCreateResult() {
          code = "0",
          message = e.Message,
          reqCode = param.reqCode,
          data = "null"
        };
      }
    }

    public RcsTaskCancelResult CancelTask(RcsTaskCancelParams param)
    {
      if (param.reqCode == null || param.reqCode == "") {
        param.reqCode = System.Guid.NewGuid().ToString();
      }

      var json = JsonSerializer.Serialize(param);
      var content = new StringContent(json, Encoding.UTF8);

      _logger.Save(Log.From(
        Log.UseClass("rcs.tasks"),
        Log.UseOperation("cancel"),
        Log.UseIndex("0"),
        Log.UseMessage("收到 RCS 取消任务"),
        Log.UseData(json),
        Log.UseInfo()
      ));

      try {
        var response = _client.PostAsync("/rcs/services/rest/hikRpcService/cancelTask", content)
          .GetAwaiter().GetResult();
        var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        _logger.Save(Log.From(
          Log.UseClass("rcs.tasks"),
          Log.UseOperation("cancel"),
          Log.UseIndex("0"),
          Log.UseMessage("RCS 任务取消成功"),
          Log.UseData(result),
          Log.UseSuccess()
        ));

        return JsonSerializer.Deserialize<RcsTaskCancelResult>(result);
      } catch (Exception e) {
        _logger.Save(Log.From(
          Log.UseClass("rcs.tasks"),
          Log.UseOperation("cancel"),
          Log.UseIndex("0"),
          Log.UseMessage("RCS 任务取消失败"),
          Log.UseData(e.Message),
          Log.UseDanger()
        ));

        return new RcsTaskCancelResult() {
          code = "0",
          message = e.Message,
          reqCode = param.reqCode,
        };
      }
    }

    private void NotifyDoorTask(string doorId, string uuid, string action)
    {
      if (uuid == "" || uuid == "A001") {
        return;
      }

      var json = JsonSerializer.Serialize(new {
        deviceType = "door",
        deviceIndex = doorId,
        actionStatus = action,
        uuid = uuid,
      });
      var content = new StringContent(json, Encoding.UTF8);

      try {
        var response = _client.PostAsync("/rcs/services/rest/liftCtlService/notifyExcuteResultInfo", content).GetAwaiter().GetResult();
        var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        _cap.Publish(RcsNotifiedEvent.Message, new RcsNotifiedEvent(doorId, action, uuid, result));
      } catch (Exception e) {
        _cap.Publish(RcsNotifiedFailedEvent.Message, new RcsNotifiedFailedEvent(doorId, action, uuid, e.Message));
      }
    }

    public void NotifyDoorOpened(string doorId, string uuid)
      => NotifyDoorTask(doorId, uuid, "1");

    public void NotifyDoorClosing(string doorId, string uuid)
      => NotifyDoorTask(doorId, uuid, "2");
  }
}
