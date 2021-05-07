using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Midos.Services.Http;
using System.Text.Json.Serialization;

namespace Namei.Wcs.Api
{
  public class RcsTaskCreateParams
  {
    public string ReqCode { get; set; }

    public string TaskTyp { get; set; }

    public string PodCode { get; set; }

    public string Priority { get; set; }

    public string TaskCode { get; set; }

    public string AgvCode { get; set; }

    public string Data { get; set; }

    public List<PositionCodePath> PositionCodePath { get; set; }
  }

  public class RcsTaskContinueParams
  {
    public string ReqCode { get; set; }

    public string PodCode { get; set; }

    public string TaskCode { get; set; }

    public string AgvCode { get; set; }

    public PositionCodePath NextPositionCode { get; set; }
  }

  public class PositionCodePath
  {
    public string PositionCode { get; set; }

    public string Type { get; set; }
  }

  public class RcsTaskCreateResult
  {
    public string Code { get; set; } = "0";

    public string Message { get; set; } = "";

    public string ReqCode { get; set; }

    public string Data { get; set; } = "";
  }

  public class RcsTaskCancelParams
  {
    public string ReqCode { get; set; }

    public string AgvCode { get; set; }

    public string TaskCode { get; set; }
  }

  public class RcsTaskCancelResult
  {
    public string Code { get; set; }

    public string Message { get; set; }

    public string ReqCode { get; set; }
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
    private readonly HttpClient _client;

    private readonly IHttpService _httpService;

    private readonly Logger _logger;

    private readonly Config _config;

    public RcsService(
      IHttpClientFactory factory,
      Config config,
      Logger logger,
      IHttpService httpService
    ) {
      _logger = logger;
      _config = config;
      _client = factory.CreateClient();
      _httpService = httpService;
      _client.Timeout = new TimeSpan(0, 0, 10);
      _client.BaseAddress = new System.Uri(config.RcsUrl);
      _client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json)
      );
    }

    public class ToDataNameParams
    {
      [JsonPropertyName("value")]
      public string Value { get; set; }
    }

    public string ToDataName(string mapData)
    {
      var url = $"{_config.NameiCommonUrl}/rcs/mapDataName/find";

      return _httpService.Post<ToDataNameParams, ToDataNameParams>(url, new() { Value = mapData }).Value;
    }

    public RcsTaskCreateResult CreateTask(RcsTaskCreateParams param)
    {
      if (param.ReqCode == null || param.ReqCode == "") {
        param.ReqCode = System.Guid.NewGuid().ToString();
      }

      foreach (var position in param.PositionCodePath) {
        if (position.PositionCode != "" && position.PositionCode != null) {
          position.PositionCode = ToDataName(position.PositionCode);
        }
      }

      var scope = _logger.UseScope(
        klass: "rcs.tasks",
        operation: "create",
        index: "0"
      );
      var json = JsonSerializer.Serialize(param);
      var content = new StringContent(json, Encoding.UTF8);
      var result = new RcsTaskCreateResult() {
        ReqCode = param.ReqCode
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

        result.Message = e.Message;
      }

      return result;
    }

    public RcsTaskCreateResult ContinueTask(RcsTaskContinueParams param)
    {
      if (param.ReqCode == null || param.ReqCode == "") {
        param.ReqCode = System.Guid.NewGuid().ToString();
      }

      var json = JsonSerializer.Serialize(param);
      var content = new StringContent(json, Encoding.UTF8);
      var scope = _logger.UseScope(
        klass: "rcs.tasks",
        operation: "continue",
        index: "0"
      );
      var result = new RcsTaskCreateResult() {
        ReqCode = param.ReqCode
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

        result.Message = e.Message;
      }

      return result;
    }

    public RcsTaskCancelResult CancelTask(RcsTaskCancelParams param)
    {
      if (param.ReqCode == null || param.ReqCode == "") {
        param.ReqCode = System.Guid.NewGuid().ToString();
      }

      var json = JsonSerializer.Serialize(param);
      var content = new StringContent(json, Encoding.UTF8);
      var scope = _logger.UseScope(
        klass: "rcs.tasks",
        operation: "cancel",
        index: "0"
      );
      var result = new RcsTaskCancelResult() {
        ReqCode = param.ReqCode
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

        result.Message = e.Message;
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
        uuid,
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
