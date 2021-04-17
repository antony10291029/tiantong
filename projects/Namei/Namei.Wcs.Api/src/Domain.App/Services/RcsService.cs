using DotNetCore.CAP;
using System;
using System.Collections.Generic;
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
    public string code { get; set; } = "0";

    public string message { get; set; } = "";

    public string reqCode { get; set; }

    public string data { get; set; } = "";
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

  public interface IRcsService
  {
    RcsTaskCreateResult CreateTask(RcsTaskCreateParams param);

    RcsTaskCreateResult ContinueTask(RcsTaskContinueParams param);

    RcsTaskCancelResult CancelTask(RcsTaskCancelParams param);

    void NotifyDoorOpened(string doorId, string uuid);

    void NotifyDoorClosing(string doorId, string uuid);

  }

  public class RcsService: IRcsService
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

      var scope = _logger.UseScope(
        klass: "rcs.tasks",
        operation: "create",
        index: "0"
      );
      var json = JsonSerializer.Serialize(param);
      var content = new StringContent(json, Encoding.UTF8);
      var result = new RcsTaskCreateResult() {
        reqCode = param.reqCode
      };

      scope.Info("收到 RCS 任务", json);

      try {
        var response = _client.PostAsync("/rcs/services/rest/hikRpcService/genAgvSchedulingTask", content)
          .GetAwaiter().GetResult();
        json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        scope.Success("RCS 任务创建成功", json);

        result = JsonSerializer.Deserialize<RcsTaskCreateResult>(json);
      } catch (Exception e) {
        scope.Danger("RCS 任务创建失败", e.Message);

        result.message = e.Message;
      }

      return result;
    }

    public RcsTaskCreateResult ContinueTask(RcsTaskContinueParams param)
    {
      if (param.reqCode == null || param.reqCode == "") {
        param.reqCode = System.Guid.NewGuid().ToString();
      }

      var json = JsonSerializer.Serialize(param);
      var content = new StringContent(json, Encoding.UTF8);
      var scope = _logger.UseScope(
        klass: "rcs.tasks",
        operation: "continue",
        index: "0"
      );
      var result = new RcsTaskCreateResult() {
        reqCode = param.reqCode
      };

      scope.Info("收到 RCS 触发任务", json);

      try {
        var response = _client.PostAsync("/rcs/services/rest/hikRpcService/continueTask", content)
          .GetAwaiter().GetResult();
        json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        scope.Success("RCS 触发创建成功", json);

        result = JsonSerializer.Deserialize<RcsTaskCreateResult>(json);
      } catch (Exception e) {
        scope.Danger("RCS 任务触发失败", e.Message);

        result.message = e.Message;
      }

      return result;
    }

    public RcsTaskCancelResult CancelTask(RcsTaskCancelParams param)
    {
      if (param.reqCode == null || param.reqCode == "") {
        param.reqCode = System.Guid.NewGuid().ToString();
      }

      var json = JsonSerializer.Serialize(param);
      var content = new StringContent(json, Encoding.UTF8);
      var scope = _logger.UseScope(
        klass: "rcs.tasks",
        operation: "cancel",
        index: "0"
      );
      var result = new RcsTaskCancelResult() {
        reqCode = param.reqCode
      };

      scope.Info("收到 RCS 取消任务", json);

      try {
        var response = _client.PostAsync("/rcs/services/rest/hikRpcService/cancelTask", content)
          .GetAwaiter().GetResult();
        json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        scope.Success("RCS 任务取消成功", json);

        return JsonSerializer.Deserialize<RcsTaskCancelResult>(json);
      } catch (Exception e) {
        scope.Danger("RCS 任务取消失败", e.Message);

        result.message = e.Message;
      }

      return result;
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
      var scope = _logger.UseScope(
        klass: "wcs.door",
        operation: "notify",
        index: uuid
      );

      scope.Info("正在通知 RCS 任务完成", json);

      try {
        var response = _client.PostAsync("/rcs/services/rest/liftCtlService/notifyExcuteResultInfo", content).GetAwaiter().GetResult();
        var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        scope.Success("已通知 RCS 任务完成", result);

      } catch (Exception e) {
        scope.Danger("通知 RCS 任务完成失败", e.Message);
      }
    }

    public void NotifyDoorOpened(string doorId, string uuid)
      => NotifyDoorTask(doorId, uuid, "1");

    public void NotifyDoorClosing(string doorId, string uuid)
      => NotifyDoorTask(doorId, uuid, "2");
  }
}
