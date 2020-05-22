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
    private PlcManager _plcManager;

    private StateLogger _stateLogger;

    private PlcRepository _plcRepository;

    public PlcWorkerController(
      PlcManager plcManager,
      StateLogger stateLogger,
      PlcRepository plcRepository
    ) {
      _plcManager = plcManager;
      _stateLogger = stateLogger;
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
      var plc = _plcRepository.EnsureFind(param.plc_id);
      PlcWorker worker;

      try {
        worker = PlcBuilder.Build(plc);
      } catch (Exception e) {
        return FailureOperation(e.Message);
      }

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
      var workers = plcs.Select(plc => PlcBuilder.Build(plc)).ToArray();

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
      var plc = _plcRepository.EnsureFind(param.plc_id);
      var worker = PlcBuilder.Build(plc);

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

    public class SetParams<T>
    {
      public string plc { get; set; }

      public string state { get; set; }

      public T value { get; set; }
    }

    public class SetByIdParams<T>
    {
      public int plc_id { get; set; }

      public int state_id { get; set; }

      public T value { get; set; }
    }

    [HttpPost]
    [Route("states/uint16/set")]
    public object SetUInt16([FromBody] SetParams<ushort> param)
    {
      _plcManager.Get(param.plc).UShort(param.state).Set(param.value);

      return SuccessOperation("数据已写入");
    }

    [HttpPost]
    [Route("states/uint16/set-by-id")]
    public object SetUInt16ById([FromBody] SetByIdParams<ushort> param)
    {
      _plcManager.Get(param.plc_id).UShort(param.state_id).Set(param.value);

      return SuccessOperation("数据已写入");
    }

    [HttpPost]
    [Route("states/int32/set")]
    public object SetInt32ById([FromBody] SetParams<int> param)
    {
      _plcManager.Get(param.plc).Int32(param.state).Set(param.value);

      return SuccessOperation("数据已写入");
    }

    [HttpPost]
    [Route("states/int32/set-by-id")]
    public object SetInt32([FromBody] SetByIdParams<int> param)
    {
      _plcManager.Get(param.plc_id).Int32(param.state_id).Set(param.value);

      return SuccessOperation("数据已写入");
    }

    [HttpPost]
    [Route("states/string/set")]
    public object SetString([FromBody] SetParams<string> param)
    {
      _plcManager.Get(param.plc).String(param.state).Set(param.value);

      return SuccessOperation("数据已写入");
    }

    [HttpPost]
    [Route("states/string/set-by-id")]
    public object SetStringByName([FromBody] SetByIdParams<string> param)
    {
      _plcManager.Get(param.plc_id).String(param.state_id).Set(param.value);

      return SuccessOperation("数据已写入");
    }

    [HttpPost]
    [Route("states/uint16/get")]
    public object GetUInt16ByName([FromBody] SetParams<string> param)
    {
      return new {
        value = _plcManager.Get(param.plc).UInt16(param.state).Get()
      };
    }

    [HttpPost]
    [Route("states/int32/get")]
    public object GetInt32([FromBody] SetParams<string> param)
    {
      return new {
        value = _plcManager.Get(param.plc).Int32(param.state).Get()
      };
    }

    [HttpPost]
    [Route("states/string/get")]
    public object GetStrinbByName([FromBody] SetParams<string> param)
    {
      return new {
        value = _plcManager.Get(param.plc).String(param.state).Get()
      };
    }

  }

}
