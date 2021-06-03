using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Namei.Wcs.Api
{
  public class RcsTaskCreateParams
  {
    public string ReqCode { get; set; }

    public string ReqTime { get; set; }

    public string ClientCode { get; set; }

    public string TokenCode { get; set; }

    public string TaskTyp { get; set; }

    public string SceneTyp { get; set; }

    public string CtnrTyp { get; set; }

    public string CtnrCode { get; set; }

    public string WbCode { get; set; }

    public string PodCode { get; set; }

    public string PodDir { get; set; }

    public string PodTyp { get; set; }

    public string MaterialLot { get; set; }

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

  public class RcsDoorNotifyResult
  {
    public int Code { get; set; }

    public string Message { get; set; }
  }

  public interface IRcsService
  {
    Task<RcsTaskCreateResult> CreateTask(RcsTaskCreateParams param);

    Task<RcsTaskCreateResult> ContinueTask(RcsTaskContinueParams param);

    Task<RcsTaskCancelResult> CancelTask(RcsTaskCancelParams param);

    Task<RcsDoorNotifyResult> NotifyDoorOpened(string doorId, string uuid);

    Task<RcsDoorNotifyResult> NotifyDoorClosing(string doorId, string uuid);
  }

  public class RcsService: IRcsService
  {
    private readonly HttpClient _client;

    private readonly Logger _logger;

    public RcsService(
      IHttpClientFactory factory,
      Config config,
      Logger logger
    ) {
      _logger = logger;
      _client = factory.CreateClient();
      _client.Timeout = new TimeSpan(0, 0, 10);
      _client.BaseAddress = new Uri(config.ApiGatewayUrl);
    }

    public class ToDataNameParams
    {
      public string Value { get; set; }
    }

    public async Task<RcsTaskCreateResult> CreateTask(RcsTaskCreateParams param)
    {
      HttpResponseMessage response;

      var url = "/rcs/agc-tasks/create";
      var scope = _logger.UseScope(
        klass: "rcs.tasks",
        operation: "create",
        index: "0"
      );

      scope.Info("收到 RCS 任务", param);

      if (string.IsNullOrWhiteSpace(param.ReqCode)) {
        param.ReqCode = Guid.NewGuid().ToString();
      }

      try {
        response = await _client.PostAsJsonAsync(url, param);
      } catch (Exception e) {
        scope.Danger("RCS 任务请求下发失败", e.Message);

        return new() { Code = "-1", Message = e.Message };
      }

      var result = await response.Content.ReadFromJsonAsync<RcsTaskCreateResult>();

      scope.Success("RCS 任务请求下发成功", result);

      return result;
    }

    public async Task<RcsTaskCreateResult> ContinueTask(RcsTaskContinueParams param)
    {
      HttpResponseMessage response;

      var url = "/rcs/agc-tasks/continue";
      var scope = _logger.UseScope(
        klass: "rcs.tasks",
        operation: "continue",
        index: "0"
      );

      scope.Info("收到 RCS 触发任务", param);

      if (param.ReqCode == null || param.ReqCode == "") {
        param.ReqCode = Guid.NewGuid().ToString();
      }

      try {
        response = await _client.PostAsJsonAsync(url, param);
      } catch (Exception e) {
        scope.Danger("RCS 任务触发失败", e.Message);

        return new() {
          Code = "-1",
          Message = e.Message
        };
      }

      var result = await response.Content.ReadFromJsonAsync<RcsTaskCreateResult>();

      scope.Success("RCS 触发创建成功", result);

      return result;
    }

    public async Task<RcsTaskCancelResult> CancelTask(RcsTaskCancelParams param)
    {
      HttpResponseMessage response;

      var url = "/rcs/agc-tasks/cancel";
      var scope = _logger.UseScope(
        klass: "rcs.tasks",
        operation: "cancel",
        index: "0"
      );

      scope.Info("收到 RCS 取消任务", param);

      if (param.ReqCode == null || param.ReqCode == "") {
        param.ReqCode = Guid.NewGuid().ToString();
      }

      try {
        response = await _client.PostAsJsonAsync(url, param);
      } catch (Exception e) {
        scope.Danger("RCS 任务取消失败", e.Message);

        return new() { Code = "-1", Message = e.Message };
      }

      var result = await response.Content.ReadFromJsonAsync<RcsTaskCancelResult>();

      scope.Success("RCS 任务取消成功", result);

      return result;
    }

    private async Task<RcsDoorNotifyResult> NotifyDoorTask(string doorId, string uuid, string action)
    {
      HttpResponseMessage response;

      var url = "/rcs/doors/notify";
      var param = new {
        deviceType = "door",
        deviceIndex = doorId,
        actionStatus = action,
        uuid,
      };
      var scope = _logger.UseScope(
        klass: "wcs.door",
        operation: "notify",
        index: uuid
      );

      scope.Info("正在通知 RCS 任务完成", param);

      if (uuid == "" || uuid == "A001") {
        return new() { Code = 0, Message = "success" };
      }

      try {
        response = await _client.PostAsJsonAsync(url, param);
      } catch (Exception e) {
        scope.Danger("通知 RCS 任务完成失败", e.Message);

        return new() { Code = -1, Message = e.Message };
      }

      var result = await response.Content.ReadFromJsonAsync<RcsDoorNotifyResult>(new(
        JsonSerializerDefaults.Web
      ));

      scope.Success("已通知 RCS 任务完成", result);

      return result;
    }

    public Task<RcsDoorNotifyResult> NotifyDoorOpened(string doorId, string uuid)
      => NotifyDoorTask(doorId, uuid, "1");

    public Task<RcsDoorNotifyResult> NotifyDoorClosing(string doorId, string uuid)
      => NotifyDoorTask(doorId, uuid, "2");
  }
}
