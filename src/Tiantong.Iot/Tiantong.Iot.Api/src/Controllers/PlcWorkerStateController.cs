using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Iot.Api
{
  [Route("/plc-states")]
  public class PlcWorkerStateController: BaseController
  {
    private PlcManager _manager;

    public PlcWorkerStateController(PlcManager manager)
    {
      _manager = manager;
    }

    private void HandleSet<T>(string plc, string state, T value)
    {
      _manager.Get(plc).Set<T>(state, value);
    }

    private T HandleGet<T>(string plc, string state)
    {
      return _manager.Get(plc).Get<T>(state);
    }

    public class GetParams<T>
    {
      public string plc { get; set; }

      public string state { get; set; }
    }

    public class SetParams<T>: GetParams<T>
    {
      public T value { get; set; }
    }

    [HttpPost]
    [Route("bool/get")]
    public object BoolGet([FromBody] GetParams<bool> param)
    {
      return new {
        value = HandleGet<bool>(param.plc, param.state)
      };
    }

    [HttpPost]
    [Route("bool/set")]
    public object BoolSet([FromBody] SetParams<bool> param)
    {
      HandleSet<bool>(param.plc, param.state, param.value);

      return SuccessOperation("数据已写入");
    }

    [HttpPost]
    [Route("uint16/get")]
    public object UInt16Get([FromBody] GetParams<ushort> param)
    {
      return new {
        value = HandleGet<ushort>(param.plc, param.state)
      };
    }

    [HttpPost]
    [Route("uint16/set")]
    public object UIntSet16([FromBody] SetParams<ushort> param)
    {
      HandleSet<ushort>(param.plc, param.state, param.value);

      return SuccessOperation("数据已写入");
    }

    [HttpPost]
    [Route("uint32/get")]
    public object Int32Get([FromBody] GetParams<int> param)
    {
      return new {
        value = HandleGet<int>(param.plc, param.state)
      };
    }

    [HttpPost]
    [Route("uint32/set")]
    public object Int3Set2([FromBody] SetParams<int> param)
    {
      HandleSet<int>(param.plc, param.state, param.value);

      return SuccessOperation("数据已写入");
    }

    [HttpPost]
    [Route("string/get")]
    public object StringGet([FromBody] GetParams<string> param)
    {
      return new {
        value = HandleGet<string>(param.plc, param.state)
      };
    }

    [HttpPost]
    [Route("string/set")]
    public object StriSetng([FromBody] SetParams<string> param)
    {
      HandleSet<string>(param.plc, param.state, param.value);

      return SuccessOperation("数据已写入");
    }

    public class SetStringByIdParams
    {
      public string plc { get; set; }

      public string state { get; set; }

      public string value { get; set; }
    }

    public class GetStringParams
    {
      public string plc { get; set; }

      public string state { get; set; }
    }

    [HttpPost]
    [Route("get-string")]
    public object GetString([FromBody] GetStringParams param)
    {
      var value = _manager.Get(param.plc).GetString(param.state);

      return new { value };
    }

    [HttpPost]
    [Route("set-string")]
    public object SetString([FromBody] SetStringByIdParams param)
    {
      _manager.Get(param.plc).SetString(param.state, param.value);

      return SuccessOperation("数据已写入");
    }
  }
}
