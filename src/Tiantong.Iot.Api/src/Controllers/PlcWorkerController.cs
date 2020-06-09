using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Iot.Api
{
  [Route("/plc-workers")]
  public class PlcWorkerController: BaseController
  {
    private PlcBuilder _builder;

    private PlcManager _plcManager;

    private PlcRepository _plcRepository;

    public PlcWorkerController(
      PlcBuilder builder,
      PlcManager plcManager,
      PlcRepository plcRepository
    ) {
      _builder = builder;
      _plcManager = plcManager;
      _plcRepository = plcRepository;
    }

    public class FindParams
    {
      public int plc_id { get; set; }
    }

    [HttpPost]
    [Route("run")]
    public object Run([FromBody] FindParams param)
    {
      var plc = _plcRepository.EnsureGetWithRelationships(param.plc_id);
      PlcWorker worker;

      worker = _builder.BuildWorker(plc);

      if (_plcManager.Run(worker)) {
        return SuccessOperation("PLC 开始运行");
      } else {
        return FailureOperation("PLC 正在运行中");
      }
    }

    [HttpPost]
    [Route("stop")]
    public object Stop([FromBody] FindParams param)
    {
      if (_plcManager.Stop(param.plc_id)) {
        return SuccessOperation("PLC 停止运行");
      } else {
        return FailureOperation("PLC 未运行");
      }
    }

    [HttpPost]
    [Route("start-all")]
    public object StartAll()
    {
      var plcs = _plcRepository.AllWithRelationships();
      var workers = plcs.Select(plc => _builder.BuildWorker(plc)).ToArray();

      foreach (var worker in workers) {
        _plcManager.Run(worker);
      }

      return SuccessOperation("所有设备已开始运行");
    }

    [HttpPost]
    [Route("stop-all")]
    public object StopAll()
    {
      _plcManager.Stop();

      return SuccessOperation("所有设备已停止运行");
    }

    [HttpPost]
    [Route("test")]
    public object Test([FromBody] FindParams param)
    {
      var plc = _plcRepository.EnsureGetWithRelationships(param.plc_id);
      var worker = _builder.BuildWorker(plc);

      try {
        worker.Test();
      } catch (Exception e) {
        return FailureOperation($"通信失败: {e.Message}");
      }

      return SuccessOperation("PLC 连接测试成功");
    }

    [HttpPost]
    [Route("is-running")]
    public object IsRunning([FromBody] FindParams param)
    {
      try {
        _plcManager.Get(param.plc_id);

        return new { is_running = true };
      } catch {
        return new { is_running = false };
      }
    }

    [HttpPost]
    [Route("current-values")]
    public Dictionary<string, string> GetCurrentValue([FromBody] FindParams param)
    {
      try {
        return _plcManager.Get(param.plc_id).GetCurrentStateValues();
      } catch {
        throw new HttpException("数据不存在", 400);
      }
    }

  }

}
