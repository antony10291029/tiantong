using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;
using System;

namespace Tiantong.Iot.Api
{
  [Route("/plc-workers/debug")]
  public class PlcWorkerDebugController: BaseController
  {
    private PlcManager _manager;

    private PlcBuilder _plcBuilder;

    private PlcRepository _plcRepository;

    public PlcWorkerDebugController(
      PlcManager manager,
      PlcBuilder plcBuilder,
      PlcRepository plcRepository
    ) {
      _manager = manager;
      _plcBuilder = plcBuilder;
      _plcRepository = plcRepository;
    }

    public string ResolveGet(int plcId, IState state)
    {
      var value = "";

      if (_manager.Has(plcId)) {
        value = _manager.Get(plcId).GetString(state);
      } else {
        var plc = _plcRepository.EnsureFind(plcId);
        var worker = _plcBuilder.BuildWorker(plc);

        try {
          worker.Connect();
          value = worker.GetString(state);
        } finally {
          worker.Close();
        }
      }

      return $"读取数据：{state.Address()} => {value}";
    }

    public string ResolveSet(int plcId, IState state, string value)
    {
      if (_manager.Has(plcId)) {
         _manager.Get(plcId).SetString(state, value);
      } else {
        var plc = _plcRepository.EnsureFind(plcId);
        var worker = _plcBuilder.BuildWorker(plc);

        try {
          worker.Connect();
          worker.SetString(state, value);

          if (state is IState<string>) {
            value = value.Substring(0, state.Length());
          }
        } finally {
          worker.Close();
        }
      }

      return $"写入数据：{state.Address()} <= {value}";
    }

    public class GetParams
    {
      public int plc_id { get; set; }

      public string address { get; set; }

      public int length { get; set; }
    }

    public class SetParams: GetParams
    {
      public string value { get; set; }
    }

    [HttpPost]
    [Route("uint16/get")]
    public object GetUInt16([FromBody] GetParams param)
    {
      var state = new StateUInt16()
        .Address(param.address);

      return new {
        message = ResolveGet(param.plc_id, state)
      };
    }

    [HttpPost]
    [Route("int32/get")]
    public object GetInt32([FromBody] GetParams param)
    {
      var state = new StateInt32()
        .Address(param.address);

      return new {
        message = ResolveGet(param.plc_id, state)
      };
    }

    [HttpPost]
    [Route("string/get")]
    public object GetString([FromBody] GetParams param)
    {
      var state = new StateString()
        .Address(param.address)
        .Length(param.length);

      return new {
        message = ResolveGet(param.plc_id, state)
      };
    }

    [HttpPost]
    [Route("uint16/set")]
    public object SetUInt16([FromBody] SetParams param)
    {
      var state = new StateUInt16()
        .Address(param.address);

      return new {
        message = ResolveSet(param.plc_id, state, param.value)
      };
    }

    [HttpPost]
    [Route("int32/set")]
    public object SetInt32([FromBody] SetParams param)
    {
      var state = new StateInt32()
        .Address(param.address);

      return new {
        message = ResolveSet(param.plc_id, state, param.value)
      };
    }

    [HttpPost]
    [Route("string/set")]
    public object SetString([FromBody] SetParams param)
    {
      var state = new StateString()
        .Address(param.address)
        .Length(param.length);

      return new {
        message = ResolveSet(param.plc_id, state, param.value)
      };
    }

  }

}
